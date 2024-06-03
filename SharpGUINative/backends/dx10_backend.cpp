#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX10

#include "backends.hpp"
#include "dx10_backend.hpp"
#include "win32_backend.hpp"

#include <d3d10.h>
#include "status_util.hpp"
#include "kiero.h"

#define CIMGUI_USE_DX10
#include "cimgui/cimgui_impl.h"

typedef HRESULT(__stdcall* PresentFunc) (IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
typedef HRESULT(__stdcall* ResizeBuffersFunc) (IDXGISwapChain* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags);

namespace Backends::DX10
{
	bool hookInit = false;

	PresentFunc oPresent;
	ResizeBuffersFunc oResizeBuffers;

	ID3D10Device* pDevice;
	ID3D10RenderTargetView* pMainRenderTargetView;

	HWND outputWindow;

	void CreateMainRenderTargetView(IDXGISwapChain* pSwapChain);

	HRESULT __stdcall hkPresent(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags);
	HRESULT __stdcall hkResizeBuffers(IDXGISwapChain* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags);
}

void Backends::DX10::CreateMainRenderTargetView(IDXGISwapChain* pSwapChain)
{
	ID3D10Texture2D* pBackBuffer;
	pSwapChain->GetBuffer(0, __uuidof(ID3D10Texture2D), (void**)&pBackBuffer);

	if (pBackBuffer == NULL)
	{
		// Shouldnt happen
		__debugbreak();
		return;
	}

	Backends::DX10::pDevice->CreateRenderTargetView(pBackBuffer, NULL, &Backends::DX10::pMainRenderTargetView);
	pBackBuffer->Release();
}

HRESULT __stdcall Backends::DX10::hkPresent(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags)
{
	if (!Backends::DX10::hookInit)
	{
		// Return if we can't get the device
		if (!SUCCEEDED(pSwapChain->GetDevice(__uuidof(ID3D10Device), (void**)&Backends::DX10::pDevice)))
		{
			return Backends::DX10::oPresent(pSwapChain, SyncInterval, Flags);
		}

		// Get swapchain description
		DXGI_SWAP_CHAIN_DESC swapChainDesc;
		pSwapChain->GetDesc(&swapChainDesc);

		// Get output window from swapchain description
		Backends::DX10::outputWindow = swapChainDesc.OutputWindow;

		// Create new render target view
		CreateMainRenderTargetView(pSwapChain);

		Backends::InitImGui();
		Backends::Win32::Initialize(Backends::DX10::outputWindow);
		ImGui_ImplDX10_Init(Backends::DX10::pDevice);

		Backends::DX10::hookInit = true;
	}

	Backends::Win32::NewFrame();
	ImGui_ImplDX10_NewFrame();
	igNewFrame();

	Backends::RenderGUI();

	igRender();

	Backends::DX10::pDevice->OMSetRenderTargets(1, &Backends::DX10::pMainRenderTargetView, NULL);
	ImGui_ImplDX10_RenderDrawData(igGetDrawData());
	return Backends::DX10::oPresent(pSwapChain, SyncInterval, Flags);
}

HRESULT __stdcall Backends::DX10::hkResizeBuffers(IDXGISwapChain* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags)
{
	if (Backends::DX10::pMainRenderTargetView)
	{
		Backends::DX10::pDevice->OMSetRenderTargets(0, NULL, NULL);
		Backends::DX10::pMainRenderTargetView->Release();
	}

	HRESULT hr = Backends::DX10::oResizeBuffers(pSwapChain, BufferCount, Width, Height, NewFormat, SwapChainFlags);

	CreateMainRenderTargetView(pSwapChain);

	Backends::DX10::pDevice->OMSetRenderTargets(1, &Backends::DX10::pMainRenderTargetView, NULL);

	return hr;
}

Backends::BackendType Backends::DX10Backend::GetType()
{
	return Backends::BackendType_DX10;
}

void Backends::DX10Backend::InitializeBackend()
{
	KIERO_CHECK_STATUS(kiero::init(kiero::RenderType::D3D10), "Kiero failed to init");

	KIERO_CHECK_STATUS(kiero::bind(8, (void**)&Backends::DX10::oPresent, Backends::DX10::hkPresent), "Kiero failed to bind Present")
	KIERO_CHECK_STATUS(kiero::bind(13, (void**)&Backends::DX10::oResizeBuffers, Backends::DX10::hkResizeBuffers), "Kiero failed to bind ResizeBuffers")
}

void Backends::DX10Backend::ShutdownBackend()
{
	kiero::shutdown();

	ImGui_ImplDX10_Shutdown();
	Backends::Win32::Shutdown();

	Backends::DX10::pDevice->Release();
	Backends::DX10::pMainRenderTargetView->Release();

	Backends::ShutdownImGui();

	Backends::DX10::hookInit = false;
}
#endif