#pragma once

#include "backends/backends.hpp"

namespace SharpGUI
{
	Backends::BackendType GetBackendType();

	Backends::BackendType GetCurrentBackend();

	bool Initialize(Backends::BackendType backendType);
	bool Initialize();

	bool Shutdown();

	void SetHandleInput(bool handleInput);
	bool GetHandleInput();
}