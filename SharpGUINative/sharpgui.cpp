#include "sharpgui.hpp"
#include "exports.hpp"

#include "sharpconfig.hpp"
#include "backends/backends.hpp"

#include "backends/dx9_backend.hpp"
#include "backends/dx10_backend.hpp"
#include "backends/dx11_backend.hpp"
#include "backends/dx12_backend.hpp"
#include "backends/win32_backend.hpp"
#include "backends/opengl_backend.hpp"
#include "backends/overlay_backend.hpp"

#if !SHARPGUI_DISABLE_CONSOLE
#include "logger.hpp"
#include "magic_enum.hpp"
#endif

Backends::BackendType SharpGUI::GetBackendType()
{
	Backends::BackendType type = Backends::BackendType::BackendType_None;


#define CHECK_FOR_DLL(dll, backendEnum)				\
	if (GetModuleHandle(TEXT(dll)) != NULL)			\
	{												\
		type = backendEnum;							\
	}

#if SHARPGUI_INCLUDE_OPENGL
	CHECK_FOR_DLL("opengl32.dll", Backends::BackendType::BackendType_OpenGL);
#endif

#if SHARPGUI_INCLUDE_DX9
	CHECK_FOR_DLL("d3d9.dll", Backends::BackendType::BackendType_DX9);
#endif

#if SHARPGUI_INCLUDE_DX10
	CHECK_FOR_DLL("d3d10.dll", Backends::BackendType::BackendType_DX10);
#endif

#if SHARPGUI_INCLUDE_DX11
	CHECK_FOR_DLL("d3d11.dll", Backends::BackendType::BackendType_DX11);
#endif

#if SHARPGUI_INCLUDE_DX12
	CHECK_FOR_DLL("d3d12.dll", Backends::BackendType::BackendType_DX12);
#endif

#undef CHECK_FOR_DLL

#if SHARPGUI_INCLUDE_OVERLAY
	if (type == Backends::BackendType::BackendType_None)
	{
		type = Backends::BackendType::BackendType_Overlay;
	}
#endif

	return type;
}

Backends::BackendType SharpGUI::GetCurrentBackend()
{
	if (Backends::currentBackend == nullptr)
		return Backends::BackendType_None;

	return Backends::currentBackend->GetType();
}

Backends::Backend* GetBackend(Backends::BackendType type)
{
	Backends::Backend* backendInstance = nullptr;

#define CHECK_TYPE(enumName, className)									\
case enumName:															\
	backendInstance = (Backends::Backend*)new className();				\
	break;																\

	switch (type)
	{
#if SHARPGUI_INCLUDE_DX9
		CHECK_TYPE(Backends::BackendType::BackendType_DX9, Backends::DX9Backend)
#endif

#if SHARPGUI_INCLUDE_DX10
		CHECK_TYPE(Backends::BackendType::BackendType_DX10, Backends::DX10Backend)
#endif

#if SHARPGUI_INCLUDE_DX11
		CHECK_TYPE(Backends::BackendType::BackendType_DX11, Backends::DX11Backend)
#endif

#if SHARPGUI_INCLUDE_DX12
		CHECK_TYPE(Backends::BackendType::BackendType_DX12, Backends::DX12Backend)
#endif

#if SHARPGUI_INCLUDE_OPENGL
		CHECK_TYPE(Backends::BackendType::BackendType_OpenGL, Backends::OpenGLBackend)
#endif

#if SHARPGUI_INCLUDE_OVERLAY
		CHECK_TYPE(Backends::BackendType::BackendType_Overlay, Backends::OverlayBackend)
#endif

	default:
		break;
	}

	return backendInstance;
}

bool SharpGUI::Initialize(Backends::BackendType backendType)
{
	// Check if current backend is initialized
	if (Backends::currentBackend != nullptr && Backends::currentBackend->IsInitialized())
		return false;

#if !SHARPGUI_DISABLE_CONSOLE
#if SHARPGUI_AUTO_OPEN_CONSOLE
	Log::OpenConsole();
#endif

	Log::LogLine("Initializing SharpGUINative with Backend: " + std::string(magic_enum::enum_name(backendType)) + ".");
#endif

	Backends::currentBackend = GetBackend(backendType);

	// Check if backend is found
	if (Backends::currentBackend == nullptr)
	{
		Log::LogLine("Couldn't find Backend...");
		return false;
	}

	Backends::currentBackend->Initialize();

	return true;
}

bool SharpGUI::Initialize()
{
	return Initialize(GetBackendType());
}

bool SharpGUI::Shutdown()
{
	if (Backends::currentBackend != nullptr && !Backends::currentBackend->IsInitialized())
		return false;

#if !SHARPGUI_DISABLE_CONSOLE
	Log::LogLine("Shutting down SharpGUI...");
#endif

	Backends::currentBackend->Shutdown();

	return true;
}

void SharpGUI::SetHandleInput(bool handleInput)
{
	if (Backends::currentBackend != nullptr && !Backends::currentBackend->IsInitialized())
		return;

	Backends::currentBackend->SetHandleInput(handleInput);
}

bool SharpGUI::GetHandleInput()
{
	if (Backends::currentBackend != nullptr && !Backends::currentBackend->IsInitialized())
		return false;

	return Backends::currentBackend->GetHandleInput();
}
