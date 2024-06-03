namespace JNNJMods.CrabCheat.Rendering;

public class GUITab
{
	public bool Enabled = true;

	public string Name;
	public TabID Id;

	public GUITab(string name, TabID id)
	{
		Name = name;
		Id = id;
	}
}
