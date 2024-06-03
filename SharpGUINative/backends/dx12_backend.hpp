#pragma once

#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX12

#include "win32_backend.hpp"

namespace Backends
{
	class DX12Backend : public Win32Backend
	{
	public:
		Backends::BackendType GetType() override;

	protected:
		void InitializeBackend() override;
		void ShutdownBackend() override;
	};
}

#endif