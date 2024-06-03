#pragma once

// Graphics API Settings
#define SHARPGUI_INCLUDE_DX9		0
#define SHARPGUI_INCLUDE_DX10		0
#define SHARPGUI_INCLUDE_DX11		1
#define SHARPGUI_INCLUDE_DX12		0
#define SHARPGUI_INCLUDE_OVERLAY	1

#define SHARPGUI_INCLUDE_OPENGL		0

// Console settings
#define SHARPGUI_DISABLE_CONSOLE	0

// If enabled this will cause the console to open when you initialize SharpGUI
// otherwise it will only be opened when an error occurs
#if _DEBUG
#define SHARPGUI_AUTO_OPEN_CONSOLE	1
#else
#define SHARPGUI_AUTO_OPEN_CONSOLE	0
#endif


#if SHARPGUI_INCLUDE_DX9
#define KIERO_INCLUDE_D3D9			1
#endif

#if SHARPGUI_INCLUDE_DX10
#define KIERO_INCLUDE_D3D10			1
#endif

#if SHARPGUI_INCLUDE_DX11
#define KIERO_INCLUDE_D3D11			1
#endif

#if SHARPGUI_INCLUDE_DX12
#define KIERO_INCLUDE_D3D12			1
#endif

#if SHARPGUI_INCLUDE_OPENGL
#define KIERO_INCLUDE_OPENGL		1
#endif
