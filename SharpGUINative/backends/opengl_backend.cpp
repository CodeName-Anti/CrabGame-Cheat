#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_OPENGL

#include "backends.hpp"
#include "opengl_backend.hpp"
#include "win32_backend.hpp"

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

#include "status_util.hpp"
#include "minhook/include/MinHook.h"

#define CIMGUI_USE_OPENGL2
#define CIMGUI_USE_OPENGL3
#include "cimgui/cimgui_impl.h"

#define IMGL3W_IMPL
#include "cimgui/imgui_impl_opengl3_loader.h"

#include "logger.hpp"

#define GL_SHADING_LANGUAGE_VERSION 0x8B8C

typedef bool(__stdcall* WglSwapBuffersFunc)(HDC hDc);

namespace Backends::OpenGL
{
	bool initialized = false;
	HWND window = NULL;

	bool initHook = false;
	bool newOpenGL = false;
	
	WglSwapBuffersFunc* swapBuffersAddress = NULL;
	WglSwapBuffersFunc oWglSwapBuffers;

	HGLRC wglContext;

	bool __stdcall hkWglSwapBuffers(HDC hDc);
	WglSwapBuffersFunc* GetWglSwapBuffers();
}

bool __stdcall Backends::OpenGL::hkWglSwapBuffers(HDC hDc)
{
	if (!initHook)
	{
		if (WindowFromDC(hDc) == window)
			return oWglSwapBuffers(hDc);

		window = WindowFromDC(hDc);


		int res = imgl3wInit();

		if (res != 0)
		{
			window = nullptr;

			return oWglSwapBuffers(hDc);

#if !SHARPGUI_DISABLE_CONSOLE
			Log::LogLine("loader cant init code: " + res);
#endif
		}

		Backends::InitImGui();
		Backends::Win32::Initialize(Backends::OpenGL::window);

		GLint iMajor;
		GLint iMinor;

		glGetIntegerv(GL_MAJOR_VERSION, &iMajor);
		glGetIntegerv(GL_MINOR_VERSION, &iMinor);

		// Check if OpenGL version is above 3.0
		if ((iMajor * 10 + iMinor) >= 30)
			newOpenGL = true;

		if (newOpenGL)
		{
			ImGui_ImplOpenGL3_Init(nullptr);
		}
		else
		{
			ImGui_ImplOpenGL2_Init();
		}

		wglContext = wglCreateContext(hDc);

		initHook = true;
	}

	HGLRC originalWglContext = wglGetCurrentContext();

	wglMakeCurrent(hDc, wglContext);

	if (newOpenGL)
		ImGui_ImplOpenGL3_NewFrame();
	else
		ImGui_ImplOpenGL2_NewFrame();
	
	Backends::Win32::NewFrame();
	igNewFrame();

	Backends::RenderGUI();

	igRender();
	
	if (newOpenGL)
		ImGui_ImplOpenGL3_RenderDrawData(igGetDrawData());
	else
		ImGui_ImplOpenGL2_RenderDrawData(igGetDrawData());

	wglMakeCurrent(hDc, originalWglContext);

	return oWglSwapBuffers(hDc);
}

WglSwapBuffersFunc* Backends::OpenGL::GetWglSwapBuffers()
{
	auto hMod = GetModuleHandleA("OPENGL32.dll");
	if (!hMod) return nullptr;

	return (WglSwapBuffersFunc*)GetProcAddress(hMod, "wglSwapBuffers");
}

Backends::BackendType Backends::OpenGLBackend::GetType()
{
	return Backends::BackendType_OpenGL;
}

void Backends::OpenGLBackend::InitializeBackend()
{
	Backends::OpenGL::swapBuffersAddress = Backends::OpenGL::GetWglSwapBuffers();

	MH_CHECK_STATUS(MH_Initialize(), "Minhook failed to init")

	MH_CHECK_STATUS(MH_CreateHook(Backends::OpenGL::swapBuffersAddress, Backends::OpenGL::hkWglSwapBuffers, (void**)&Backends::OpenGL::oWglSwapBuffers), "Minhook failed to create SwapBuffers hook")
	MH_CHECK_STATUS(MH_EnableHook(Backends::OpenGL::swapBuffersAddress), "Minhook failed to enable SwapBuffers hook")
}

void Backends::OpenGLBackend::ShutdownBackend()
{
	MH_CHECK_STATUS(MH_DisableHook(Backends::OpenGL::swapBuffersAddress), "Minhook failed to disable SwapBuffers hook")
	MH_CHECK_STATUS(MH_RemoveHook(Backends::OpenGL::swapBuffersAddress), "Minhook failed to remove SwapBuffers hook")

	MH_CHECK_STATUS(MH_Uninitialize(), "Minhook failed to uninit");

	if (Backends::OpenGL::newOpenGL)
	{
		ImGui_ImplOpenGL3_Shutdown();
	}
	else
	{
		ImGui_ImplOpenGL2_Shutdown();
	}

	wglDeleteContext(Backends::OpenGL::wglContext);

	Backends::Win32::Shutdown();

	Backends::ShutdownImGui();

	Backends::OpenGL::initHook = false;
}
#endif