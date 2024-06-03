namespace SharpGUI;

public class GUI
{
	public static event Action OnInitImGui;
	public static event Action OnRender;

	public static bool HandleInput
	{
		get
		{
			return _HandleInput;
		}
		set
		{
			_HandleInput = value;
			SharpGUINatives.SetHandleInput(value);
		}
	}

	private static bool _HandleInput;

	private static void PrepareInit()
	{
		GUIHelper.LoadLibraries();

		SharpGUINatives.SetInitImGuiCallback(GUIHelper.CreateNativeDelegate(InitImGuiCallback));
		SharpGUINatives.SetRenderCallback(GUIHelper.CreateNativeDelegate(RenderCallback));
	}

	public static bool Initialize(BackendType backendType)
	{
		PrepareInit();
		return SharpGUINatives.InitializeSharpGUIBackend(backendType);
	}

	public static bool Initialize()
	{
		PrepareInit();
		return SharpGUINatives.InitializeSharpGUI();
	}

	private static void InitImGuiCallback()
	{
		OnInitImGui?.Invoke();
	}

	private static void RenderCallback()
	{
		OnRender?.Invoke();
	}

	public static void Shutdown()
	{
		SharpGUINatives.ShutdownSharpGUI();
	}
}
