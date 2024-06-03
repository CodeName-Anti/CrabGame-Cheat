#pragma once

#include "sharpconfig.hpp"

#if SHARPGUI_INCLUDE_DX9

namespace Backends
{
	class DX9Backend : public Backend
	{
	public:
		Backends::BackendType GetType() override;

	protected:
		void InitializeBackend() override;
		void ShutdownBackend() override;
	};
}

#endif