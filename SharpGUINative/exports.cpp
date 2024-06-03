#include "exports.hpp"
#include "sharpgui.hpp"

namespace Interop
{
	SharpGUICallback initImGuiCallback;
	SharpGUICallback renderCallback;
}

bool __stdcall InitializeSharpGUI()
{
	return SharpGUI::Initialize();
}

bool __stdcall InitializeSharpGUIBackend(int backendType)
{
	return SharpGUI::Initialize((Backends::BackendType)backendType);
}

bool __stdcall ShutdownSharpGUI()
{
	return SharpGUI::Shutdown();
}

void __stdcall SetInitImGuiCallback(Interop::SharpGUICallback initImGuiCallback)
{
	Interop::initImGuiCallback = initImGuiCallback;
}

void __stdcall SetRenderCallback(Interop::SharpGUICallback renderCallback)
{
	Interop::renderCallback = renderCallback;
}

bool __stdcall GetHandleInput()
{
	return SharpGUI::GetHandleInput();
}

void __stdcall SetHandleInput(bool handleInput)
{
	SharpGUI::SetHandleInput(handleInput);
}
