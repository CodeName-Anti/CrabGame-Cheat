#define CIMGUI_DEFINE_ENUMS_AND_STRUCTS
#include "cimgui.h"

#ifdef CIMGUI_USE_WIN32
CIMGUI_API bool ImGui_ImplWin32_Init(void* hwnd);
CIMGUI_API bool ImGui_ImplWin32_InitForOpenGL(void* hwnd);
CIMGUI_API void ImGui_ImplWin32_Shutdown(void);
CIMGUI_API void ImGui_ImplWin32_NewFrame(void);
CIMGUI_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd,UINT msg,WPARAM wParam,LPARAM lParam);
CIMGUI_API void ImGui_ImplWin32_EnableDpiAwareness(void);
CIMGUI_API float ImGui_ImplWin32_GetDpiScaleForHwnd(void* hwnd);
CIMGUI_API float ImGui_ImplWin32_GetDpiScaleForMonitor(void* monitor);
CIMGUI_API void ImGui_ImplWin32_EnableAlphaCompositing(void* hwnd);

#endif
#ifdef CIMGUI_USE_DX9

typedef struct IDirect3DDevice9 IDirect3DDevice9;
struct IDirect3DDevice9;CIMGUI_API bool ImGui_ImplDX9_Init(IDirect3DDevice9* device);
CIMGUI_API void ImGui_ImplDX9_Shutdown(void);
CIMGUI_API void ImGui_ImplDX9_NewFrame(void);
CIMGUI_API void ImGui_ImplDX9_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API bool ImGui_ImplDX9_CreateDeviceObjects(void);
CIMGUI_API void ImGui_ImplDX9_InvalidateDeviceObjects(void);

#endif
#ifdef CIMGUI_USE_DX10

typedef struct ID3D10Device ID3D10Device;
struct ID3D10Device;CIMGUI_API bool ImGui_ImplDX10_Init(ID3D10Device* device);
CIMGUI_API void ImGui_ImplDX10_Shutdown(void);
CIMGUI_API void ImGui_ImplDX10_NewFrame(void);
CIMGUI_API void ImGui_ImplDX10_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API void ImGui_ImplDX10_InvalidateDeviceObjects(void);
CIMGUI_API bool ImGui_ImplDX10_CreateDeviceObjects(void);

#endif
#ifdef CIMGUI_USE_DX11

typedef struct ID3D11Device ID3D11Device;
typedef struct ID3D11DeviceContext ID3D11DeviceContext;
struct ID3D11Device;
struct ID3D11DeviceContext;CIMGUI_API bool ImGui_ImplDX11_Init(ID3D11Device* device,ID3D11DeviceContext* device_context);
CIMGUI_API void ImGui_ImplDX11_Shutdown(void);
CIMGUI_API void ImGui_ImplDX11_NewFrame(void);
CIMGUI_API void ImGui_ImplDX11_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API void ImGui_ImplDX11_InvalidateDeviceObjects(void);
CIMGUI_API bool ImGui_ImplDX11_CreateDeviceObjects(void);

#endif
#ifdef CIMGUI_USE_DX12

typedef struct ID3D12Device ID3D12Device;
typedef struct ID3D12DescriptorHeap ID3D12DescriptorHeap;
typedef struct ID3D12GraphicsCommandList ID3D12GraphicsCommandList;
typedef struct D3D12_CPU_DESCRIPTOR_HANDLE D3D12_CPU_DESCRIPTOR_HANDLE;
typedef struct D3D12_GPU_DESCRIPTOR_HANDLE D3D12_GPU_DESCRIPTOR_HANDLE;
struct ID3D12Device;
struct ID3D12DescriptorHeap;
struct ID3D12GraphicsCommandList;
struct D3D12_CPU_DESCRIPTOR_HANDLE;
struct D3D12_GPU_DESCRIPTOR_HANDLE;CIMGUI_API bool ImGui_ImplDX12_Init(ID3D12Device* device,int num_frames_in_flight,DXGI_FORMAT rtv_format,ID3D12DescriptorHeap* cbv_srv_heap,D3D12_CPU_DESCRIPTOR_HANDLE font_srv_cpu_desc_handle,D3D12_GPU_DESCRIPTOR_HANDLE font_srv_gpu_desc_handle);
CIMGUI_API void ImGui_ImplDX12_Shutdown(void);
CIMGUI_API void ImGui_ImplDX12_NewFrame(void);
CIMGUI_API void ImGui_ImplDX12_RenderDrawData(ImDrawData* draw_data,ID3D12GraphicsCommandList* graphics_command_list);
CIMGUI_API void ImGui_ImplDX12_InvalidateDeviceObjects(void);
CIMGUI_API bool ImGui_ImplDX12_CreateDeviceObjects(void);

#endif
#ifdef CIMGUI_USE_OPENGL2
CIMGUI_API bool ImGui_ImplOpenGL2_Init(void);
CIMGUI_API void ImGui_ImplOpenGL2_Shutdown(void);
CIMGUI_API void ImGui_ImplOpenGL2_NewFrame(void);
CIMGUI_API void ImGui_ImplOpenGL2_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API bool ImGui_ImplOpenGL2_CreateFontsTexture(void);
CIMGUI_API void ImGui_ImplOpenGL2_DestroyFontsTexture(void);
CIMGUI_API bool ImGui_ImplOpenGL2_CreateDeviceObjects(void);
CIMGUI_API void ImGui_ImplOpenGL2_DestroyDeviceObjects(void);

#endif
#ifdef CIMGUI_USE_OPENGL3
CIMGUI_API bool ImGui_ImplOpenGL3_Init(const char* glsl_version);
CIMGUI_API void ImGui_ImplOpenGL3_Shutdown(void);
CIMGUI_API void ImGui_ImplOpenGL3_NewFrame(void);
CIMGUI_API void ImGui_ImplOpenGL3_RenderDrawData(ImDrawData* draw_data);
CIMGUI_API bool ImGui_ImplOpenGL3_CreateFontsTexture(void);
CIMGUI_API void ImGui_ImplOpenGL3_DestroyFontsTexture(void);
CIMGUI_API bool ImGui_ImplOpenGL3_CreateDeviceObjects(void);
CIMGUI_API void ImGui_ImplOpenGL3_DestroyDeviceObjects(void);

#endif
