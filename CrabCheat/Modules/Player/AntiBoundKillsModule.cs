using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using UnityEngine;

namespace JNNJMods.CrabCheat.Modules.Player;

[CheatModule]
public class AntiBoundKillsModule : Module
{
	public float killHeight = float.NaN;
	public bool Enabled;

	public AntiBoundKillsModule() : base("AntiBound Kills", TabID.Player)
	{

	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			killHeight = float.NaN;
	}

	public override void Update()
	{
		if (!InGame)
			return;

		if (!Enabled)
			return;

		if (float.IsNaN(killHeight))
		{
			MonoBehaviourPublicSikiUnique killBounds = Object.FindObjectOfType<MonoBehaviourPublicSikiUnique>();

			if (killBounds != null)
			{
				killHeight = killBounds.killHeight;
			}
			else
				return;
		}

		Vector3 pos = Instances.PlayerMovement.GetRb().position;

		if (pos.y < killHeight + 2)
		{
			//Makes you slide to the sides
			Instances.PlayerMovement.GetRb().velocity = Vector3.Exclude(Vector3.up, Instances.PlayerMovement.GetRb().velocity);

			pos.y = killHeight + 2;

			//Float above KillBounds
			Instances.PlayerMovement.GetRb().position = pos;
		}

	}

}
