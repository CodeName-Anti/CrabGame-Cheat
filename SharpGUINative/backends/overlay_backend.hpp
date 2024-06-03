#pragma once

#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_OVERLAY

#include "backends.hpp"
#include "win32_backend.hpp"

namespace Backends
{
	class OverlayBackend : public Win32Backend
	{
	public:
		Backends::BackendType GetType() override;

	protected:
		void InitializeBackend() override;
		void ShutdownBackend() override;
	};
}

#endif