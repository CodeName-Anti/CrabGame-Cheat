#pragma once

namespace Interop
{
	typedef void (*SharpGUICallback)();

	extern SharpGUICallback initImGuiCallback;
	extern SharpGUICallback renderCallback;
}

extern "C"
{
	__declspec(dllexport) bool __stdcall InitializeSharpGUI();
	__declspec(dllexport) bool __stdcall InitializeSharpGUIBackend(int backendType);
	__declspec(dllexport) bool __stdcall ShutdownSharpGUI();

	__declspec(dllexport) void __stdcall SetInitImGuiCallback(Interop::SharpGUICallback initImGuiCallback);
	__declspec(dllexport) void __stdcall SetRenderCallback(Interop::SharpGUICallback renderCallback);

	__declspec(dllexport) bool __stdcall GetHandleInput();
	__declspec(dllexport) void __stdcall SetHandleInput(bool handleInput);
}