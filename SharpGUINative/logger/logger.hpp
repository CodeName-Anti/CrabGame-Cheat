#pragma once

#include "sharpconfig.hpp"
#if !SHARPGUI_DISABLE_CONSOLE

#include <string>

namespace Log
{
    void OpenConsole();
    void CloseConsole();

    void Log(std::string message, bool timestamp);

    void LogLine(std::string message, bool timestamp = true);
}
#endif
