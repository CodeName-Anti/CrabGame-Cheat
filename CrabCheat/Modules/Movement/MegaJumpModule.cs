using ImGuiNET;
using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using JNNJMods.CrabCheat.Util;

namespace JNNJMods.CrabCheat.Modules.Movement;

[CheatModule]
public class MegaJumpModule : Module
{
	public bool Enabled;

	private bool init;

	private float jumpForce;

	public MegaJumpModule() : base("Mega Jump", TabID.Movement)
	{

	}

	public override void RenderGUIElements()
	{
		if (!ImGui.Checkbox(Name, ref Enabled))
			return;

		UnityMainThreadDispatcher.Enqueue(OnToggleChanged);
	}

	private void OnToggleChanged()
	{
		if (!InGame)
			return;

		if (Enabled)
			Instances.PlayerMovement.SetJumpForce(jumpForce * 2f);
		else
			Instances.PlayerMovement.SetJumpForce(jumpForce);
	}

	public override void Update()
	{
		if (!InGame)
			return;

		if (!init)
		{
			init = true;

			jumpForce = Instances.PlayerMovement.GetJumpForce();
		}

		if (!Enabled)
			return;

		OnToggleChanged();
	}

}
