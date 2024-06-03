//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using JNNJMods.CrabCheat.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JNNJMods.CrabCheat.Rendering.Outline;

public class Outline : MonoBehaviour
{
	private static HashSet<Mesh> registeredMeshes = [];

	public enum Mode
	{
		OutlineAll,
		OutlineVisible,
		OutlineHidden,
		OutlineAndSilhouette,
		SilhouetteOnly
	}

	public Mode OutlineMode
	{
		get => outlineMode;
		set
		{
			outlineMode = value;
			needsUpdate = true;
		}
	}

	public Color OutlineColor
	{
		get => outlineColor;
		set
		{
			outlineColor = value;
			needsUpdate = true;
		}
	}

	public float OutlineWidth
	{
		get => outlineWidth;
		set
		{
			outlineWidth = value;
			needsUpdate = true;
		}
	}

	[Serializable]
	private class ListVector3
	{
		public List<Vector3> data;
	}

	private Mode outlineMode;

	private Color outlineColor = Color.white;

	private float outlineWidth = 2f;

	private bool precomputeOutline = false;

	private List<Mesh> bakeKeys = [];

	private List<ListVector3> bakeValues = [];

	private Renderer[] renderers;
	private Material outlineMaskMaterial;
	private Material outlineFillMaterial;

	private bool needsUpdate;

	public Outline(IntPtr handle) : base(handle) { }

	private void Awake()
	{

		// Cache renderers
		renderers = GetComponentsInChildren<Renderer>();

		// Instantiate outline materials
		outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
		outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

		outlineMaskMaterial.name = "OutlineMask (Instance)";
		outlineFillMaterial.name = "OutlineFill (Instance)";

		// Retrieve or generate smooth normals
		LoadSmoothNormals();

		// Apply material properties immediately
		needsUpdate = true;
	}

	private void OnEnable()
	{
		foreach (Renderer renderer in renderers)
		{

			// Append outline shaders
			List<Material> materials = renderer.sharedMaterials.ToList();

			materials.Add(outlineMaskMaterial);
			materials.Add(outlineFillMaterial);

			renderer.materials = materials.ToArray();
		}
	}

	private void OnValidate()
	{

		// Update material properties
		needsUpdate = true;

		// Clear cache when baking is disabled or corrupted
		if ((!precomputeOutline && bakeKeys.Count != 0) || bakeKeys.Count != bakeValues.Count)
		{
			bakeKeys.Clear();
			bakeValues.Clear();
		}

		// Generate smooth normals when baking is enabled
		if (precomputeOutline && bakeKeys.Count == 0)
		{
			Bake();
		}
	}

	private void Update()
	{
		if (needsUpdate)
		{
			needsUpdate = false;

			UpdateMaterialProperties();
		}
	}

	private void OnDisable()
	{
		foreach (Renderer renderer in renderers)
		{

			// Remove outline shaders
			List<Material> materials = renderer.sharedMaterials.ToList();

			materials.Remove(outlineMaskMaterial);
			materials.Remove(outlineFillMaterial);

			renderer.materials = materials.ToArray();
		}
	}

	private void OnDestroy()
	{

		// Destroy material instances
		Destroy(outlineMaskMaterial);
		Destroy(outlineFillMaterial);
	}

	private void Bake()
	{

		// Generate smooth normals for each mesh
		HashSet<Mesh> bakedMeshes = [];

		foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
		{

			// Skip duplicates
			if (!bakedMeshes.Add(meshFilter.sharedMesh))
			{
				continue;
			}

			// Serialize smooth normals
			List<Vector3> smoothNormals = SmoothNormals(meshFilter.sharedMesh);

			bakeKeys.Add(meshFilter.sharedMesh);
			bakeValues.Add(new ListVector3() { data = smoothNormals });
		}
	}

	private void LoadSmoothNormals()
	{

		// Retrieve or generate smooth normals
		foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
		{

			// Skip if smooth normals have already been adopted
			if (!registeredMeshes.Add(meshFilter.sharedMesh))
			{
				continue;
			}

			// Retrieve or generate smooth normals
			int index = bakeKeys.IndexOf(meshFilter.sharedMesh);
			List<Vector3> smoothNormals = index >= 0 ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

			// Store smooth normals in UV3
			meshFilter.sharedMesh.SetUVs(3, smoothNormals.ToIL2CPP());
		}

		// Clear UV3 on skinned mesh renderers
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
			{
				skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
			}
		}
	}

	private List<Vector3> SmoothNormals(Mesh mesh)
	{

		// Group vertices by location
		IEnumerable<IGrouping<Vector3, KeyValuePair<Vector3, int>>> groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

		// Copy normals to a new list
		List<Vector3> smoothNormals = new(mesh.normals);

		// Average normals for grouped vertices
		foreach (IGrouping<Vector3, KeyValuePair<Vector3, int>> group in groups)
		{

			// Skip single vertices
			if (group.Count() == 1)
			{
				continue;
			}

			// Calculate the average normal
			Vector3 smoothNormal = Vector3.zero;

			foreach (KeyValuePair<Vector3, int> pair in group)
			{
				smoothNormal += mesh.normals[pair.Value];
			}

			smoothNormal.Normalize();

			// Assign smooth normal to each vertex
			foreach (KeyValuePair<Vector3, int> pair in group)
			{
				smoothNormals[pair.Value] = smoothNormal;
			}
		}

		return smoothNormals;
	}

	private void UpdateMaterialProperties()
	{

		// Apply properties according to mode
		outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

		switch (outlineMode)
		{
			case Mode.OutlineAll:
				outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
				outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
				outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
				break;

			case Mode.OutlineVisible:
				outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
				outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
				outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
				break;

			case Mode.OutlineHidden:
				outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
				outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
				outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
				break;

			case Mode.OutlineAndSilhouette:
				outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
				outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
				outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
				break;

			case Mode.SilhouetteOnly:
				outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
				outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
				outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
				break;
		}
	}
}
