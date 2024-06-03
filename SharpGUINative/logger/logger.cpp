#include "sharpconfig.hpp"
#if !SHARPGUI_DISABLE_CONSOLE

#include "logger.hpp"

#include <string>
#include <fstream>
#include <iostream>
#include <filesystem>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>

using std::string;
using std::cout;
using std::endl;

namespace Log
{
    bool open;

    std::stringstream cacheStream;

    FILE* consoleIn;
    FILE* consoleOut;
    FILE* consoleErr;

    void OpenConsole()
    {
        if (open)
            return;

        AllocConsole();

        freopen_s(&consoleIn, "CONIN$", "r", stdin);
        freopen_s(&consoleOut, "CONOUT$", "w", stdout);
        freopen_s(&consoleErr, "CONOUT$", "w", stderr);

        open = true;

        if (cacheStream.rdbuf()->in_avail() != 0)
        {
            cout << cacheStream.rdbuf();
        }
    }

    void CloseConsole()
    {
        if (!open)
            return;

        fclose(consoleIn);
        fclose(consoleOut);
        fclose(consoleErr);

        FreeConsole();

        open = false;
    }

    static std::tm LocalTime_Xp(std::time_t time)
    {
        std::tm bt{};
        localtime_s(&bt, &time);
        return bt;
    }

    static string TimeStamp(const std::string& fmt)
    {
        auto bt = LocalTime_Xp(std::time(0));
        char buf[64];
        return { buf, std::strftime(buf, sizeof(buf), fmt.c_str(), &bt) };
    }

    static std::string ModifyMessage(std::string message, bool timestamp)
    {
        if (!timestamp)
            return message;

        return "[" + TimeStamp("%T") + "] " + message;
    }

    static std::ostream& GetOutputStream()
    {
        if (open)
            return cout;
        else
            return cacheStream;
    }

    void Log::Log(std::string message, bool timestamp)
    {
        GetOutputStream() << ModifyMessage(message, timestamp);
    }

    void Log::LogLine(std::string message, bool timestamp)
    {
        GetOutputStream() << ModifyMessage(message, timestamp) << endl;
    }
}
#endif
