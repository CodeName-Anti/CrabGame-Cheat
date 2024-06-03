#include "backends.hpp"

#include "sharpgui.hpp"

#define CIMGUI_DEFINE_ENUMS_AND_STRUCTS
#include "cimgui/cimgui.h"

#include "exports.hpp"

#include <string>

namespace Backends
{
	Backend* currentBackend;
}

Backends::BackendType Backends::Backend::GetType()
{
	return BackendType::BackendType_None;
}

void Backends::Backend::Initialize()
{
	if (this->initialized)
		return;

	this->InitializeBackend();

	initialized = true;
}

bool Backends::Backend::IsInitialized()
{
	return this->initialized;
}

void Backends::Backend::Shutdown()
{
	if (!this->initialized)
		return;

	this->ShutdownBackend();

	initialized = false;
}

bool Backends::Backend::GetHandleInput()
{
	return this->handleInput;
}

void Backends::InitImGui()
{
	igCreateContext(nullptr);
	ImGuiIO* io = igGetIO();
	io->ConfigFlags = ImGuiConfigFlags_NavEnableKeyboard;
	io->IniFilename = nullptr;

	if (Interop::initImGuiCallback != nullptr)
		Interop::initImGuiCallback();
}

void Backends::ShutdownImGui()
{
	igDestroyContext(nullptr);
}

#if _DEBUG
static void DrawDebugWindow()
{
	static bool showGui = true;
	static bool showDemoWindow = false;

	// H is the greatest letter of all times
	if (igIsKeyPressed_Bool(ImGuiKey::ImGuiKey_H, false))
	{
		showGui = !showGui;
		SetHandleInput(showGui);
	}

	if (!showGui)
		return;

	igBegin("Debug h", nullptr, NULL);

	ImGuiIO* io = igGetIO();

	igText("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / io->Framerate, io->Framerate);

	igCheckbox("Demo window", &showDemoWindow);

	igText("Shutdown will be fixed in a later version");

	if (igButton("Shutdown", ImVec2()))
	{
		ShutdownSharpGUI();
		return;
	}

	igEnd();

	if (showDemoWindow)
	{
		igShowDemoWindow(nullptr);
	}
}
#endif

static void DrawInfoWindow()
{
	static bool infoOpen = true;
	static const char* projectUrl = "https://github.com/CodeName-Anti/SharpGUI";

	if (!infoOpen)
		return;

	if (igIsKeyPressed_Bool(ImGuiKey_B, false))
		SetHandleInput(!GetHandleInput());

	if (!igBegin("SharpGUI", nullptr, NULL))
	{
		igEnd();
		return;
	}

	igText("Welcome to SharpGUI, you haven't set your rendering callback, yet...");
	igText("In the meantime here's something useful");
	igText("Press B to toggle input");

	igSpacing();

	igText("Project link: %s", projectUrl);
	igSameLine(0.0f, -1.0f);
	if (igButton("Copy", ImVec2()))
	{
		igSetClipboardText(projectUrl);
	}

	igEnd();
}

void Backends::RenderGUI()
{
	static bool infoOpen = true;

	if (Interop::renderCallback != nullptr)
		Interop::renderCallback();
	else
		// Draw an info window if no callback is set
		DrawInfoWindow();

#if _DEBUG
	DrawDebugWindow();
#endif

}