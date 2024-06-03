#pragma once

#include "magic_enum.hpp"
#include "logger.hpp"

#include "kiero.h"
#include "minhook/include/MinHook.h"

#include "sharpconfig.hpp"

#if SHARPGUI_DISABLE_CONSOLE
	#define STATUS_UTIL_GET_MESSAGE(msg) "" // empty so we dont emit any string
#else
#define STATUS_UTIL_GET_MESSAGE(msg) #msg
#endif

#define KIERO_CHECK_STATUS(status, message) if (status_util::CheckKieroStatus(status, STATUS_UTIL_GET_MESSAGE(message))) return;
#define MH_CHECK_STATUS(status, message) if (status_util::CheckMinhookStatus(status, STATUS_UTIL_GET_MESSAGE(message))) return;

namespace status_util
{
	// return value of true means error
	inline bool CheckKieroStatus(kiero::Status::Enum status, std::string message)
	{
		if (status == kiero::Status::Success)
			return false;

#if !SHARPGUI_DISABLE_CONSOLE
		// Make sure console is open
		Log::OpenConsole();
		Log::LogLine(message + " with status " + std::string(magic_enum::enum_name(status)) + ".");
#endif
		return true;
	}

	// return value of true means error
	inline bool CheckMinhookStatus(MH_STATUS status, std::string message)
	{
		if (status == MH_STATUS::MH_OK)
			return false;

#if !SHARPGUI_DISABLE_CONSOLE
		// Make sure console is open
		Log::OpenConsole();
		Log::LogLine(message + " with status " + std::string(magic_enum::enum_name(status)) + ".");
#endif
		return true;
	}
}