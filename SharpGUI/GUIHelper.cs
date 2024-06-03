using System.Reflection;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace SharpGUI;

internal class GUIHelper
{
	private static readonly List<FreeLibrarySafeHandle> LoadedLibraries = new();

	// Cache delegates to avoid garbage collection
	private static readonly List<object> Cache = new();

	internal static void CopyResource(string name, string outputFile)
	{
		using Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(GUI).Namespace + ".Resources." + name);

		using FileStream file = File.Create(outputFile);
		resource.CopyTo(file);

		file.Flush();
		file.Close();

		resource.Close();
	}

	internal static void LoadLibraryResource(string name, string nativesFolder)
	{
		string libraryPath = Path.Combine(nativesFolder, name);

#if DEBUG
		CopyResource(name, libraryPath);
#else
		if (!File.Exists(libraryPath))
			CopyResource(name, libraryPath);
#endif

		LoadedLibraries.Add(PInvoke.LoadLibrary(libraryPath));
	}

	internal static void LoadLibraries()
	{
		// Load cimgui.dll + SharpGUINative.dll
		string nativesFolder = Path.Combine(new FileInfo(Assembly.GetCallingAssembly().Location).Directory.FullName, "natives");

		if (!Directory.Exists(nativesFolder))
		{
			Directory.CreateDirectory(nativesFolder);
		}

		LoadLibraryResource("cimgui.dll", nativesFolder);
		LoadLibraryResource("SharpGUINative.dll", nativesFolder);
	}

	internal static IntPtr CreateNativeDelegate<TDelegate>(TDelegate d) where TDelegate : notnull
	{
		IntPtr pDelegate = Marshal.GetFunctionPointerForDelegate(d);
		Cache.Add(d);
		Cache.Add(pDelegate);

		return pDelegate;
	}

}
