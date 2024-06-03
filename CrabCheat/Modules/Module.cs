using JNNJMods.CrabCheat.Rendering;
using JNNJMods.CrabCheat.Translators;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace JNNJMods.CrabCheat.Modules;

public class Module
{
	public TabID TabId { get; protected set; }

	[JsonIgnore]
	protected static bool InGame => Instances.PlayerMovement != null;

	[JsonIgnore]
	protected int ID => (int)TabId;

	public string Name { get; protected set; }

	public virtual void Init(bool json = false)
	{
	}

	public Module(string name, TabID tabId)
	{
		Name = name;
		TabId = tabId;

		Init();
	}

	public Module(string name)
	{
		Name = name;

		Init();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void RenderGUIElements() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void OnRender() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void Update() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void FixedUpdate() { }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void OnGUI() { }

}
