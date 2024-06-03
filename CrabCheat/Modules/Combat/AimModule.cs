using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using Newtonsoft.Json;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Combat;

// Temporarily disabled because it requires fixing
//[CheatModule]
public class AimModule : Module
{
	public bool Enabled;

	[JsonIgnore]
	private LayerMask AimLayers = LayerMask.GetMask("Default", "Player", "Ground");

	[JsonIgnore]
	private Camera MainCam;

	public AimModule() : base("Aim", TabID.Combat)
	{
	}

	private bool AimbotValid()
	{
		return InGame && PlayerInput.Instance.active;
	}

	public override void Update()
	{
		if (!AimbotValid())
			return;

		if (!Enabled)
			return;

		if (MainCam == null)
			MainCam = Camera.main;

		AimAt(GetClosestEnemy().head.position);
	}

	public override void RenderGUIElements()
	{
		ImGui.Checkbox("Aimbot", ref Enabled);
	}

	private void TriggerBot()
	{
		if (!IsOnEnemy())
			return;
		//TODO: IMPLEMENT
	}

	public bool IsOnEnemy()
	{
		// RayCast to see if Collider is player
		return Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 999999f, AimLayers)
			&& IsEnemy(hit);
	}

	private bool IsEnemy(RaycastHit hit)
	{
		string name = hit.transform.name.ToLower();
		return name.Contains("player") || hit.collider.gameObject.layer == AimLayers;
	}

	public bool IsVisible(Vector3 toCheck)
	{
		// Raycast to enemy position, to see if the enemy is visible
		return Physics.Linecast(Camera.main.transform.position, toCheck, out RaycastHit hit, AimLayers) && IsEnemy(hit);
	}

	private static Vector2 CalcAngle(Vector3 src, Vector3 dst)
	{
		Vector2 angle;
		Vector3 relative;
		relative = src - dst;
		float magnitude = relative.magnitude;
		float pitch = Mathf.Asin(relative.y / magnitude);
		float yaw = -Mathf.Atan2(relative.x, -relative.z);

		yaw *= Mathf.Rad2Deg;
		pitch *= Mathf.Rad2Deg;

		angle.x = pitch;
		angle.y = yaw;
		return angle;
	}

	public void AimAt(Vector3 target)
	{
		Vector3 direction = target - MainCam.transform.position;

		Quaternion rotation = Quaternion.LookRotation(target);

		Instances.PlayerMovement.GetPlayerCamTransform().rotation = rotation;

		float deltaLength = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z));
		float desiredXangle = -Mathf.Asin(direction.y / deltaLength) * (180 / Mathf.PI);

		if (desiredXangle > 90)
			desiredXangle = 90;
		else if (desiredXangle < -90)
			desiredXangle = -90;

		PlayerInput.Instance.desiredX = desiredXangle;

		//Vector3 euler = Instances.PlayerMovement.orientation.transform.eulerAngles;
		//euler.y = aimAngle.y;
		//Instances.PlayerMovement.orientation.transform.eulerAngles = euler;


		/*
		Vector3 camAngle = move.playerCam.transform.eulerAngles;
		move.xRotation = aimAngle.x;
		camAngle.y = aimAngle.y;
		move.playerCam.transform.eulerAngles = camAngle;
		*/
	}

	private PlayerManager GetClosestEnemy()
	{
		float closestEnemyDistance = float.MaxValue;
		PlayerManager bestPlayer = null;

		foreach (PlayerManager manager in GameManager.Instance.activePlayers.Values)
		{
			if (manager == null)
				continue;

			if (manager.dead)
				continue;

			if (!IsVisible(manager.head.transform.position))
				continue;

			float distance = Vector3.Distance(Instances.PlayerMovement.transform.position, manager.transform.position);

			if (distance < closestEnemyDistance)
			{
				closestEnemyDistance = distance;
				bestPlayer = manager;
			}

		}

		return bestPlayer;
	}

}
