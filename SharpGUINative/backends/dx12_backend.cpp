#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX12

#include "backends.hpp"
#include "dx12_backend.hpp"
#include "win32_backend.hpp"

#include <dxgi1_4.h>
#include <d3d12.h>
#include "status_util.hpp"
#include "kiero.h"

#include <vector>

#define CIMGUI_USE_DX12
#include "cimgui/cimgui_impl.h"

typedef HRESULT(__stdcall* PresentFunc) (IDXGISwapChain3* pSwapChain, UINT SyncInterval, UINT Flags);
typedef void(__stdcall* ExecuteCommandListsFunc) (ID3D12CommandQueue* queue, UINT NumCommandLists, ID3D12CommandList* ppCommandLists);
typedef HRESULT(__stdcall* ResizeBuffersFunc) (IDXGISwapChain3* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags);

namespace Backends::DX12
{
	bool hookInit = false;

	struct FrameContext
	{
		ID3D12CommandAllocator* command_allocator;
		ID3D12Resource* main_render_target_resource;
		D3D12_CPU_DESCRIPTOR_HANDLE main_render_target_descriptor;
	};

	std::vector<FrameContext> frameContext;
	UINT frameBufferCount;

	// render target view
	ID3D12DescriptorHeap* rtvDescriptorHeap;

	// shader resource view
	ID3D12DescriptorHeap* srvDescriptorHeap;

	ID3D12CommandQueue* commandQueue;
	ID3D12GraphicsCommandList* commandList;

	PresentFunc oPresent;
	ExecuteCommandListsFunc oExecuteCommandLists;
	ResizeBuffersFunc oResizeBuffers;

	ID3D12Device* pDevice;
	IDXGISwapChain3* pSwapchain;

	HWND outputWindow;

	bool CreateRenderTarget(IDXGISwapChain3*& pSwapChain);
	bool ClearRenderTarget();

	HRESULT __stdcall hkPresent(IDXGISwapChain3* pSwapChain, UINT SyncInterval, UINT Flags);

	void WaitForLastSubmittedFrame();
	void __stdcall hkExecuteCommandLists(ID3D12CommandQueue* queue, UINT NumCommandLists, ID3D12CommandList* ppCommandLists);

	HRESULT __stdcall hkResizeBuffers(IDXGISwapChain3* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags);
}

bool Backends::DX12::CreateRenderTarget(IDXGISwapChain3*& pSwapChain)
{
	UINT rtvDescriptorSize = pDevice->GetDescriptorHandleIncrementSize(D3D12_DESCRIPTOR_HEAP_TYPE_RTV);
	D3D12_CPU_DESCRIPTOR_HANDLE rtvCpuHandle = rtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

	for (UINT i = 0; i < frameBufferCount; i++)
	{
		frameContext[i].main_render_target_descriptor = rtvCpuHandle;
		pSwapChain->GetBuffer(i, IID_PPV_ARGS(&frameContext[i].main_render_target_resource));
		pDevice->CreateRenderTargetView(frameContext[i].main_render_target_resource, nullptr, rtvCpuHandle);
		rtvCpuHandle.ptr += rtvDescriptorSize;
	}

	return true;
}

bool Backends::DX12::ClearRenderTarget()
{
	for (FrameContext& context : frameContext)
	{
		context.main_render_target_resource->Release();
		context.main_render_target_resource = nullptr;
	}

	return true;
}

HRESULT __stdcall Backends::DX12::hkPresent(IDXGISwapChain3* pSwapChain, UINT SyncInterval, UINT Flags)
{
	if (commandQueue == nullptr)
		return oPresent(pSwapChain, SyncInterval, Flags);

	if (pSwapchain != nullptr && pSwapChain != Backends::DX12::pSwapchain)
		return oPresent(pSwapChain, SyncInterval, Flags);

	if (!hookInit)
	{
		// Return if we can't get the device
		if (FAILED(pSwapChain->GetDevice(__uuidof(ID3D12Device), (void**)&pDevice)))
		{
			return oPresent(pSwapChain, SyncInterval, Flags);
		}

		Backends::DX12::pSwapchain = pSwapChain;

		// Get swapchain description
		DXGI_SWAP_CHAIN_DESC swapChainDesc;
		pSwapChain->GetDesc(&swapChainDesc);

		outputWindow = swapChainDesc.OutputWindow;
		frameBufferCount = swapChainDesc.BufferCount;
		frameContext.clear();
		frameContext.resize(frameBufferCount);


		D3D12_DESCRIPTOR_HEAP_DESC srvHeapDesc{};
		srvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV;
		srvHeapDesc.NumDescriptors = frameBufferCount;
		srvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE;

		if (FAILED(pDevice->CreateDescriptorHeap(&srvHeapDesc, __uuidof(ID3D12DescriptorHeap), (void**)&srvDescriptorHeap)))
		{
			return oPresent(pSwapChain, SyncInterval, Flags);
		}


		D3D12_DESCRIPTOR_HEAP_DESC rtvHeapDesc{};
		rtvHeapDesc.Type = D3D12_DESCRIPTOR_HEAP_TYPE_RTV;
		rtvHeapDesc.NumDescriptors = frameBufferCount;
		rtvHeapDesc.Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE;
		rtvHeapDesc.NodeMask = 0;

		if (FAILED(pDevice->CreateDescriptorHeap(&rtvHeapDesc, __uuidof(ID3D12DescriptorHeap), (void**)&rtvDescriptorHeap)))
		{
			return oPresent(pSwapChain, SyncInterval, Flags);
		}

		if (!CreateRenderTarget(pSwapChain))
			return oPresent(pSwapChain, SyncInterval, Flags);

		ID3D12CommandAllocator* allocator;
		if (FAILED(pDevice->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&allocator))))
		{
			return oPresent(pSwapChain, SyncInterval, Flags);
		}

		for (size_t i = 0; i < frameBufferCount; i++)
		{
			if (FAILED(pDevice->CreateCommandAllocator(D3D12_COMMAND_LIST_TYPE_DIRECT, IID_PPV_ARGS(&frameContext[i].command_allocator))))
			{
				return oPresent(pSwapChain, SyncInterval, Flags);
			}
		}

		if (FAILED(pDevice->CreateCommandList(0, D3D12_COMMAND_LIST_TYPE_DIRECT, frameContext[0].command_allocator, NULL, IID_PPV_ARGS(&commandList))) || FAILED(commandList->Close()))
		{
			return oPresent(pSwapChain, SyncInterval, Flags);
		}

		Backends::InitImGui();
		Backends::Win32::Initialize(outputWindow);
	
		// ID3D12Device* device,int num_frames_in_flight,DXGI_FORMAT rtv_format,ID3D12DescriptorHeap* cbv_srv_heap,
		// D3D12_CPU_DESCRIPTOR_HANDLE font_srv_cpu_desc_handle,D3D12_GPU_DESCRIPTOR_HANDLE font_srv_gpu_desc_handle
		ImGui_ImplDX12_Init(
			pDevice,
			frameBufferCount,
			DXGI_FORMAT_R8G8B8A8_UNORM,
			srvDescriptorHeap,
			srvDescriptorHeap->GetCPUDescriptorHandleForHeapStart(),
			srvDescriptorHeap->GetGPUDescriptorHandleForHeapStart()
		);

		hookInit = true;
	}

	Backends::Win32::NewFrame();
	ImGui_ImplDX12_NewFrame();
	igNewFrame();

	Backends::RenderGUI();

	FrameContext& currentFrameContext = frameContext[pSwapChain->GetCurrentBackBufferIndex()];
	currentFrameContext.command_allocator->Reset();

	D3D12_RESOURCE_BARRIER barrier;
	barrier.Type = D3D12_RESOURCE_BARRIER_TYPE_TRANSITION;
	barrier.Flags = D3D12_RESOURCE_BARRIER_FLAG_NONE;
	barrier.Transition.pResource = currentFrameContext.main_render_target_resource;
	barrier.Transition.Subresource = D3D12_RESOURCE_BARRIER_ALL_SUBRESOURCES;
	barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_PRESENT;
	barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_RENDER_TARGET;

	commandList->Reset(currentFrameContext.command_allocator, nullptr);
	commandList->ResourceBarrier(1, &barrier);
	commandList->OMSetRenderTargets(1, &currentFrameContext.main_render_target_descriptor, FALSE, nullptr);
	commandList->SetDescriptorHeaps(1, &srvDescriptorHeap);

	igRender();
	ImGui_ImplDX12_RenderDrawData(igGetDrawData(), commandList);

	barrier.Transition.StateBefore = D3D12_RESOURCE_STATE_RENDER_TARGET;
	barrier.Transition.StateAfter = D3D12_RESOURCE_STATE_PRESENT;
	commandList->ResourceBarrier(1, &barrier);
	commandList->Close();

	commandQueue->ExecuteCommandLists(1, (ID3D12CommandList**)&commandList);

	return oPresent(pSwapChain, SyncInterval, Flags);
}

void __stdcall Backends::DX12::hkExecuteCommandLists(ID3D12CommandQueue* queue, UINT NumCommandLists, ID3D12CommandList* ppCommandLists)
{
	if (!commandQueue && queue->GetDesc().Type == D3D12_COMMAND_LIST_TYPE_DIRECT)
	{
		commandQueue = queue;
	}

	return oExecuteCommandLists(queue, NumCommandLists, ppCommandLists);
}

HRESULT __stdcall Backends::DX12::hkResizeBuffers(IDXGISwapChain3* pSwapChain, UINT BufferCount, UINT Width, UINT Height, DXGI_FORMAT NewFormat, UINT SwapChainFlags)
{
	ClearRenderTarget();

	HRESULT hr = oResizeBuffers(pSwapChain, BufferCount, Width, Height, NewFormat, SwapChainFlags);

	CreateRenderTarget(pSwapChain);

	return hr;
}

Backends::BackendType Backends::DX12Backend::GetType()
{
	return Backends::BackendType_DX12;
}

void Backends::DX12Backend::InitializeBackend()
{
	KIERO_CHECK_STATUS(kiero::init(kiero::RenderType::D3D12), "Kiero failed to init");

	KIERO_CHECK_STATUS(kiero::bind(140, (void**)&Backends::DX12::oPresent, Backends::DX12::hkPresent), "Kiero failed to bind Present")
	KIERO_CHECK_STATUS(kiero::bind(54, (void**)&Backends::DX12::oExecuteCommandLists, Backends::DX12::hkExecuteCommandLists), "Kiero failed to bind ExecuteCommandLists")
	KIERO_CHECK_STATUS(kiero::bind(145, (void**)&Backends::DX12::oResizeBuffers, Backends::DX12::hkResizeBuffers), "Kiero failed to bind ResizeBuffers")
}

void Backends::DX12Backend::ShutdownBackend()
{
	kiero::shutdown();

	ImGui_ImplDX12_Shutdown();
	Backends::Win32::Shutdown();

	Backends::ShutdownImGui();

	Backends::DX12::hookInit = false;
}
#endif