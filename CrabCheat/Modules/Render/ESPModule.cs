using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Util;
using SteamworksNative;
using System.Collections.Generic;
using UnityEngine;
using SysVector2 = System.Numerics.Vector2;

namespace JNNJMods.CrabCheat.Modules.Render;

[CheatModule]
public class ESPModule : Module
{
	public bool Enabled;

	private Camera MainCam;

	private List<ESPPlayerData> ESPData = [];

	private class ESPPlayerData
	{
		public string Name;

		public Vector3 W2SFootPosition;
		public Vector3 W2SHeadPosition;
	}

	public ESPModule() : base("ESP", TabID.Render) { }

	public override void RenderGUIElements()
	{
		ImGui.Checkbox(Name, ref Enabled);
	}

	public override void OnRender()
	{
		if (!Enabled)
			return;

		ImDrawListPtr drawList = ImGui.GetBackgroundDrawList();
		SysVector2 screenCenter = new(Screen.width / 2, Screen.height / 2);
		uint greenColor = Color.green.ToImGuiColor();

		lock (ESPData)
		{


			foreach (ESPPlayerData playerData in ESPData)
			{
				SysVector2 w2s_head = playerData.W2SHeadPosition.ToSysVec2();
				SysVector2 w2s_foot = playerData.W2SFootPosition.ToSysVec2();

				float height = w2s_foot.Y - w2s_head.Y;

				SysVector2 topLeft = new(w2s_head.X - (height / 4), Screen.height - w2s_head.Y);
				SysVector2 bottomRight = new(w2s_foot.X + (height / 4), Screen.height - w2s_foot.Y);

				CalculateRect(topLeft, bottomRight, out SysVector2 topRight, out SysVector2 bottomLeft);

				drawList.AddRect(topLeft, bottomRight, Color.green.ToImGuiColor());

				SysVector2 bottomMiddle = new(bottomLeft.X + (bottomRight.X - bottomLeft.X), bottomLeft.Y);

				drawList.AddLine(screenCenter, bottomMiddle, greenColor);
				//drawList.AddLine(screenCenter, w2s_head, greenColor);

				string name = playerData.Name;

				SysVector2 textSize = ImGui.CalcTextSize(name);
				SysVector2 textPos = topLeft + new SysVector2(0, 10) - (textSize / 2);

				drawList.AddText(textPos, greenColor, name);
			}
		}
	}

	public override void OnGUI()
	{
		if (!InGame)
		{
			ESPData.Clear();
			return;
		}

		if (!Enabled)
			return;

		if (MainCam == null)
			MainCam = Camera.main;

		lock (ESPData)
		{
			ESPData.Clear();

			foreach (PlayerManager player in GameManager.Instance.activePlayers.Values)
			{
				if (player.steamProfile.m_SteamID == SteamUser.GetSteamID().m_SteamID)
					continue;

				if (player.dead)
					continue;

				Vector3 w2s_head = MainCam.WorldToScreenPoint(player.head.position);
				Vector3 w2s_foot = MainCam.WorldToScreenPoint(player.transform.position - new Vector3(0, 1, 0));

				// Discard player if behind camera
				if (w2s_head.z < 0 || w2s_foot.z < 0)
					continue;

				ESPData.Add(new()
				{
					Name = player.username,
					W2SHeadPosition = w2s_head,
					W2SFootPosition = w2s_foot
				});
			}
		}
	}

	private static void CalculateRect(SysVector2 topLeft, SysVector2 bottomRight, out SysVector2 topRight, out SysVector2 bottomLeft)
	{
		topRight = new(bottomRight.X, topLeft.Y);
		bottomLeft = new(topLeft.X, bottomRight.Y);
	}

}
