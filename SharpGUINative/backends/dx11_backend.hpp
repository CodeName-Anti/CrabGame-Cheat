#pragma once

#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX11

#include "win32_backend.hpp"

namespace Backends
{
	class DX11Backend : public Win32Backend
	{
	public:
		Backends::BackendType GetType() override;

	protected:
		void InitializeBackend() override;
		void ShutdownBackend() override;
	};
}

#endif