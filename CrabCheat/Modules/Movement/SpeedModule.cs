using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class SpeedModule : Module
{
	public bool Enabled;
	public float SpeedAmount;

	public SpeedModule() : base("Speed", TabID.Movement)
	{

	}

	public override void RenderGUIElements()
	{
		if (ImGui.Checkbox(Name, ref Enabled))
			UnityMainThreadDispatcher.Enqueue(ToggleChanged);

		ImGui.SliderFloat("Speed Amount", ref SpeedAmount, 1, 40);
	}

	private void ToggleChanged()
	{
		if (!InGame)
			return;

		PlayerMovement move = Instances.PlayerMovement;
		if (Enabled)
		{
			move.SetMaxRunSpeed(13 * SpeedAmount);
			move.SetMaxSpeed(6.5f * SpeedAmount);
		}
		else
		{
			move.SetMaxRunSpeed(13);
			move.SetMaxSpeed(6.5f);
		}
	}

	public override void Update()
	{
		if (!InGame)
			return;

		ToggleChanged();
	}

}