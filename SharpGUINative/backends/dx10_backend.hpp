#pragma once

#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX10

#include "backends.hpp"
#include "win32_backend.hpp"

namespace Backends
{
	class DX10Backend : public Win32Backend
	{
	public:
		Backends::BackendType GetType() override;

	protected:
		void InitializeBackend() override;
		void ShutdownBackend() override;
	};
}

#endif