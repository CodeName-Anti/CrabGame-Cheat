#pragma once

#define WIN32_LEAN_AND_MEAN
#include "backends.hpp"
#include <Windows.h>

namespace Backends
{
	class Win32Backend : public Backend
	{
	public:
		void SetHandleInput(bool handleInput) override;
	};

	namespace Win32
	{
		void Initialize(HWND window);

		void NewFrame();

		void Shutdown();
	}
}