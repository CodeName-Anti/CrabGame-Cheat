#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_OVERLAY

#include "backends.hpp"
#include "overlay_backend.hpp"
#include "win32_backend.hpp"

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <dwmapi.h>
#include <d3d11.h>

#include <thread>

#define CIMGUI_DEFINE_ENUMS_AND_STRUCTS
#include "cimgui/cimgui.h"

#define CIMGUI_USE_DX11
#include "cimgui/cimgui_impl.h"

namespace Backends::Overlay
{
	bool initialized = false;
	bool runThread = false;

	bool handleInput = true;

	HANDLE thread;

	HWND targetWindow = NULL;
	HWND outputWindow = NULL;

	WORD ResizeWidth = 0;
	WORD ResizeHeight = 0;

	UINT SyncInterval = 0;

	ID3D11Device* pDevice = nullptr;
	ID3D11DeviceContext* pContext = nullptr;
	IDXGISwapChain* pSwapChain = nullptr;
	ID3D11RenderTargetView* pMainRenderTargetView = nullptr;

	LRESULT WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam);

	// Render Target
	void CreateRenderTarget();
	void CleanupRenderTarget();

	// Device
	bool CreateDevice(HWND outputWindow);
	void CleanupDevice();

	// Window stuff
	bool IsWindowAlive();
	bool UnadjustWindowRectEx(RECT* prc, LONG dwStyle, bool fMenu, LONG dwExStyle);
	void MoveOutputWindow();
	bool IsWindowFocus();

	// Renders DX11
	void RenderThread();

	// Window utilities
	struct handle_data
	{
		unsigned long process_id;
		HWND window_handle;
	};

	BOOL IsMainWindow(HWND handle);
	BOOL CALLBACK EnumWindowsCallback(HWND handle, LPARAM lParam);
	HWND FindMainWindow(unsigned long process_id);
}

LRESULT Backends::Overlay::WndProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	switch (Msg)
	{
	case WM_SIZE:
		if (Backends::Overlay::pDevice != nullptr && wParam != SIZE_MINIMIZED)
		{
			Backends::Overlay::ResizeWidth = LOWORD(lParam);
			Backends::Overlay::ResizeHeight = HIWORD(lParam);
		}
		return 0;

	case WM_SYSCOMMAND:
		if ((wParam & 0xfff0) == SC_KEYMENU)
			return 0;
		break;

	case WM_DESTROY:
		PostMessageA(Backends::Overlay::targetWindow, WM_DESTROY, 0, 0);
		PostQuitMessage(0);
		return 0;

	default:
		break;
	}
	return DefWindowProc(hWnd, Msg, wParam, lParam);
}

void Backends::Overlay::CreateRenderTarget()
{
	ID3D11Texture2D* pBackBuffer;
	Backends::Overlay::pSwapChain->GetBuffer(0, IID_PPV_ARGS(&pBackBuffer));
	
	if (pBackBuffer != nullptr)
	{
		Backends::Overlay::pDevice->CreateRenderTargetView(pBackBuffer, nullptr, &Backends::Overlay::pMainRenderTargetView);
		pBackBuffer->Release();
	}
}

void Backends::Overlay::CleanupRenderTarget()
{
	if (Backends::Overlay::pMainRenderTargetView)
	{
		Backends::Overlay::pMainRenderTargetView->Release();
		Backends::Overlay::pMainRenderTargetView = nullptr;
	}
}

bool Backends::Overlay::CreateDevice(HWND outputWindow)
{
	DXGI_SWAP_CHAIN_DESC sd;

	sd.BufferCount = 2;

	sd.BufferDesc.Width = 0;
	sd.BufferDesc.Height = 0;
	sd.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;

	sd.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;
	sd.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	sd.OutputWindow = outputWindow;
	
	sd.SampleDesc.Count = 1;
	sd.SampleDesc.Quality = 0;

	sd.Windowed = true;
	sd.SwapEffect = DXGI_SWAP_EFFECT_DISCARD;

	const UINT createDeviceFlags = 0;
	D3D_FEATURE_LEVEL featureLevel;
	const D3D_FEATURE_LEVEL featureLevelArray[] = { D3D_FEATURE_LEVEL_11_0, D3D_FEATURE_LEVEL_10_0 };

	auto createDevice = [createDeviceFlags, featureLevelArray, sd, &featureLevel](D3D_DRIVER_TYPE driverType) -> HRESULT {
		return D3D11CreateDeviceAndSwapChain(
			nullptr,
			driverType,
			NULL,
			createDeviceFlags,
			featureLevelArray,
			2,
			D3D11_SDK_VERSION,
			&sd,
			&Backends::Overlay::pSwapChain,
			&Backends::Overlay::pDevice,
			&featureLevel,
			&Backends::Overlay::pContext
		);
	};

	HRESULT res = createDevice(D3D_DRIVER_TYPE_HARDWARE);

	if (res == DXGI_ERROR_UNSUPPORTED)
		res = createDevice(D3D_DRIVER_TYPE_WARP);

	if (res != S_OK)
		return false;

	CreateRenderTarget();

	return true;
}

void Backends::Overlay::CleanupDevice()
{
	CleanupRenderTarget();

	// Way too lazy to write all these namespaces lol
#define CLEAN_D3D(x)		\
	if (x)					\
	{						\
		x->Release();		\
		x = nullptr;		\
	}

	CLEAN_D3D(Backends::Overlay::pSwapChain);
	CLEAN_D3D(Backends::Overlay::pContext);
	CLEAN_D3D(Backends::Overlay::pDevice);

#undef CLEAN_D3D
}


bool Backends::Overlay::IsWindowAlive()
{
	if (Backends::Overlay::targetWindow == NULL)
		return false;

	if (!IsWindow(Backends::Overlay::targetWindow))
		return false;

	return true;
}

bool Backends::Overlay::UnadjustWindowRectEx(RECT* prc, LONG dwStyle, bool fMenu, LONG dwExStyle)
{
	RECT rc = {};

	bool fRc = AdjustWindowRectEx(&rc, dwStyle, fMenu, dwExStyle);

	if (fRc)
	{
		prc->left -= rc.left;
		prc->top -= rc.top;
		prc->right -= rc.right;
		prc->bottom -= rc.bottom;
	}

	return fRc;
}

void Backends::Overlay::MoveOutputWindow()
{
	if (Backends::Overlay::targetWindow == NULL)
		return;

	auto& targetWindow = Backends::Overlay::targetWindow;

	LONG targetStyle = GetWindowLong(targetWindow, GWL_STYLE);
	LONG targetExStyle = GetWindowLong(targetWindow, GWL_EXSTYLE);

	RECT clientRect;
	GetWindowRect(targetWindow, &clientRect);
	UnadjustWindowRectEx(&clientRect, targetStyle, false, targetExStyle);

	LONG rectWidth = clientRect.right - clientRect.left;
	LONG rectHeight = clientRect.bottom - clientRect.top;

	SetWindowPos(Backends::Overlay::outputWindow, NULL, clientRect.left, clientRect.top, rectWidth, rectHeight, SWP_SHOWWINDOW);
}

bool Backends::Overlay::IsWindowFocus()
{
	HWND outputWindow = Backends::Overlay::outputWindow;
	HWND targetWindow = Backends::Overlay::targetWindow;

	char lpCurrentWindowUsedClass[125];
	char lpCurrentWindowClass[125];
	char lpOverlayWindowClass[125];

	const HWND hCurrentWindowUsed = GetForegroundWindow();
	if (GetClassNameA(hCurrentWindowUsed, lpCurrentWindowUsedClass, sizeof(lpCurrentWindowUsedClass)) == 0)
		return false;

	if (GetClassNameA(targetWindow, lpCurrentWindowClass, sizeof(lpCurrentWindowClass)) == 0)
		return false;

	if (GetClassNameA(outputWindow, lpOverlayWindowClass, sizeof(lpOverlayWindowClass)) == 0)
		return false;

	if (strcmp(lpCurrentWindowUsedClass, lpCurrentWindowClass) != 0 && strcmp(lpCurrentWindowUsedClass, lpOverlayWindowClass) != 0)
	{
		SetWindowLong(outputWindow, GWL_EXSTYLE, WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOOLWINDOW);
		return false;
	}

	return true;
}

void Backends::Overlay::RenderThread()
{
	WNDCLASSEX windowClass;

	windowClass.cbSize = sizeof(WNDCLASSEX);
	windowClass.style = CS_VREDRAW | CS_HREDRAW;
	windowClass.lpfnWndProc = WndProc;
	windowClass.cbClsExtra = NULL;
	windowClass.cbWndExtra = NULL;
	windowClass.hInstance = GetModuleHandle(nullptr);
	windowClass.hIcon = LoadIcon(nullptr, IDI_APPLICATION);
	windowClass.hCursor = LoadCursor(nullptr, IDC_ARROW);
	windowClass.hbrBackground = (HBRUSH)CreateSolidBrush(RGB(0, 0, 0));
	windowClass.lpszMenuName = nullptr;
	windowClass.lpszClassName = TEXT("SharpGUI Overlay");
	windowClass.hIconSm = LoadIcon(nullptr, IDI_APPLICATION);

	RegisterClassEx(&windowClass);

	HWND outputWindow = CreateWindowEx(
		WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE,
		windowClass.lpszClassName,
		windowClass.lpszClassName,
		WS_POPUP,
		0,
		0,
		100,
		100,
		nullptr,
		nullptr,
		windowClass.hInstance,
		nullptr
	);

	Backends::Overlay::outputWindow = outputWindow;

	SetLayeredWindowAttributes(outputWindow, 0, 255, LWA_ALPHA);
	const MARGINS margin = { -1, 0, 0, 0 };
	DwmExtendFrameIntoClientArea(outputWindow, &margin);

	if (!CreateDevice(outputWindow))
	{
		CleanupDevice();
		UnregisterClass(windowClass.lpszClassName, windowClass.hInstance);

		// dont call normal shutdown because it will join the thread
		Backends::Overlay::initialized = false;
		Backends::Overlay::runThread = false;

		Backends::Overlay::targetWindow = NULL;
		Backends::Overlay::outputWindow = NULL; 
		return;
	}

	ShowWindow(outputWindow, SW_SHOWDEFAULT);
	UpdateWindow(outputWindow);

	Backends::InitImGui();

	// Not using Backends::Win32 as it is for hooking only
	Backends::Win32::Initialize(Backends::Overlay::targetWindow);

	ImGui_ImplDX11_Init(Backends::Overlay::pDevice, Backends::Overlay::pContext);

	const ImVec4 clearColor = ImVec4(.2f, .0f, .0f, .2f);

	while (Backends::Overlay::runThread)
	{
		MSG msg;

		while (PeekMessage(&msg, nullptr, NULL, NULL, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);

			if (msg.message == WM_QUIT)
				Backends::Overlay::runThread = false;
		}

		if (!IsWindowAlive())
			Backends::Overlay::runThread = false;

		if (!Backends::Overlay::runThread)
			break;

		if (Backends::Overlay::ResizeWidth != 0 && Backends::Overlay::ResizeHeight != 0)
		{
			CleanupRenderTarget();
			Backends::Overlay::pSwapChain->ResizeBuffers(0, Backends::Overlay::ResizeWidth, Backends::Overlay::ResizeHeight, DXGI_FORMAT_UNKNOWN, 0);
			Backends::Overlay::ResizeWidth = Backends::Overlay::ResizeHeight = 0;

			CreateRenderTarget();
		}

		if (Backends::Overlay::targetWindow != NULL)
			MoveOutputWindow();

		const float clearColorWithAlpha[] = { clearColor.x * clearColor.w, clearColor.y * clearColor.w, clearColor.z * clearColor.w, clearColor.w };
		if (!IsWindowFocus())
		{
			Backends::Overlay::pContext->OMSetRenderTargets(1, &Backends::Overlay::pMainRenderTargetView, nullptr);
			Backends::Overlay::pContext->ClearRenderTargetView(Backends::Overlay::pMainRenderTargetView, clearColorWithAlpha);

			Backends::Overlay::pSwapChain->Present(Backends::Overlay::SyncInterval, 0);
			continue;
		}

		ImGui_ImplDX11_NewFrame();
		Backends::Win32::NewFrame();
		igNewFrame();

		Backends::RenderGUI();

		SetWindowLongW(outputWindow, GWL_EXSTYLE, WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_TRANSPARENT);

		igRender();

		Backends::Overlay::pContext->OMSetRenderTargets(1, &Backends::Overlay::pMainRenderTargetView, nullptr);
		Backends::Overlay::pContext->ClearRenderTargetView(Backends::Overlay::pMainRenderTargetView, clearColorWithAlpha);
		ImGui_ImplDX11_RenderDrawData(igGetDrawData());

		Backends::Overlay::pSwapChain->Present(Backends::Overlay::SyncInterval, 0);
	}

	Backends::Overlay::initialized = false;

	ImGui_ImplDX11_Shutdown();
	Backends::Win32::Shutdown();
	Backends::ShutdownImGui();

	CleanupDevice();
	
	DestroyWindow(Backends::Overlay::outputWindow);
	UnregisterClass(windowClass.lpszClassName, windowClass.hInstance);

	Backends::Overlay::initialized = false;
	Backends::Overlay::targetWindow = NULL;
	Backends::Overlay::outputWindow = NULL;
}

BOOL Backends::Overlay::IsMainWindow(HWND handle)
{
	return GetWindow(handle, GW_OWNER) == (HWND)0 && IsWindowVisible(handle);
}

BOOL CALLBACK Backends::Overlay::EnumWindowsCallback(HWND handle, LPARAM lParam)
{
	handle_data& data = *(handle_data*)lParam;

	unsigned long process_id = 0;
	GetWindowThreadProcessId(handle, &process_id);

	if (data.process_id != process_id || !IsMainWindow(handle))
		return TRUE;

	data.window_handle = handle;
	return FALSE;
}

HWND Backends::Overlay::FindMainWindow(unsigned long process_id)
{
	handle_data data;
	data.process_id = process_id;
	data.window_handle = 0;
	EnumWindows(EnumWindowsCallback, (LPARAM)&data);
	return data.window_handle;
}

Backends::BackendType Backends::OverlayBackend::GetType()
{
	return Backends::BackendType_Overlay;
}

void Backends::OverlayBackend::InitializeBackend()
{
	Backends::Overlay::targetWindow = Backends::Overlay::FindMainWindow(GetCurrentProcessId());

	Backends::Overlay::runThread = true;

	Backends::Overlay::thread = CreateThread(nullptr, NULL, (LPTHREAD_START_ROUTINE)Backends::Overlay::RenderThread, nullptr, NULL, nullptr);
}

void Backends::OverlayBackend::ShutdownBackend()
{
	Backends::Overlay::runThread = false;

	// Wait until thread is finished
	WaitForSingleObject(Backends::Overlay::thread, INFINITE);

	Backends::Overlay::targetWindow = NULL;
	Backends::Overlay::outputWindow = NULL;
}
#endif