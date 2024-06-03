#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX9

#include "backends.hpp"
#include "dx9_backend.hpp"
#include "win32_backend.hpp"

#include <d3d9.h>
#include "status_util.hpp"
#include "kiero.h"

#define CIMGUI_USE_DX9
#include "cimgui/cimgui_impl.h"

typedef HRESULT(__stdcall* EndSceneFunc)(IDirect3DDevice9* pDevice);
typedef HRESULT(__stdcall* ResetFunc)(IDirect3DDevice9* pDevice, D3DPRESENT_PARAMETERS* pPresentationParameters);

namespace Backends::DX9
{
	bool hookInit = false;

	EndSceneFunc oEndScene;
	ResetFunc oReset;

	HRESULT __stdcall hkEndScene(IDirect3DDevice9* pDevice);
	HRESULT __stdcall hkReset(IDirect3DDevice9* pDevice, D3DPRESENT_PARAMETERS* pPresentationParameters);
}

HRESULT __stdcall Backends::DX9::hkEndScene(IDirect3DDevice9* pDevice)
{
	if (!hookInit)
	{
		D3DDEVICE_CREATION_PARAMETERS creationParameters;
		pDevice->GetCreationParameters(&creationParameters);

		Backends::InitImGui();
		Backends::Win32::Initialize(creationParameters.hFocusWindow);
		ImGui_ImplDX9_Init(pDevice);

		hookInit = true;
	}

	Backends::Win32::NewFrame();
	ImGui_ImplDX9_NewFrame();
	igNewFrame();

	Backends::RenderGUI();

	igRender();

	ImGui_ImplDX9_RenderDrawData(igGetDrawData());

	return oEndScene(pDevice);
}

HRESULT __stdcall Backends::DX9::hkReset(IDirect3DDevice9* pDevice, D3DPRESENT_PARAMETERS* pPresentationParameters)
{
	ImGui_ImplDX9_InvalidateDeviceObjects();

	HRESULT hr = oReset(pDevice, pPresentationParameters);

	ImGui_ImplDX9_CreateDeviceObjects();

	return hr;
}

Backends::BackendType Backends::DX9Backend::GetType()
{
	return Backends::BackendType_DX9;
}

void Backends::DX9Backend::InitializeBackend()
{
	KIERO_CHECK_STATUS(kiero::init(kiero::RenderType::D3D9), "Kiero failed to init");
	
	KIERO_CHECK_STATUS(kiero::bind(42, (void**)&Backends::DX9::oEndScene, Backends::DX9::hkEndScene), "Kiero failed to bind EndScene");
	KIERO_CHECK_STATUS(kiero::bind(16, (void**)&Backends::DX9::oReset, Backends::DX9::hkReset), "Kiero failed to bind Reset");
}

void Backends::DX9Backend::ShutdownBackend()
{
	kiero::shutdown();

	ImGui_ImplDX9_Shutdown();
	Backends::Win32::Shutdown();

	Backends::Win32::Shutdown();
	Backends::ShutdownImGui();
}
#endif