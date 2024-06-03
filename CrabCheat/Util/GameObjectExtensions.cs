using System.Collections.Generic;
using UnityEngine;

namespace JNNJMods.CrabCheat.Util;

public static class GameObjectExtensions
{
	public static List<GameObject> GetChildren(this GameObject parent)
	{
		List<GameObject> children = [];

		for (int i = 0; i < parent.transform.childCount; i++)
		{
			children.Add(parent.transform.GetChild(i).gameObject);
		}

		return children;
	}
}
