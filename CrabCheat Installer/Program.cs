using Pastel;

namespace CrabGame_Cheat_Installer;

public static class Program
{
	
	public static void Main(string[] args)
	{
		WriteLine(@"

  _,,_  _     __   _,, ,  _, _  ___, 
 /  |_)'|\   '|_) /  |_|,/_,'|\' |   
'\_'| \ |-\  _|_)'\_'| |'\_  |-\ |   
   `'  `'  `'       `' `   ` '  `'   
                                     ", ConsoleColor.Red);

		Console.CursorTop++;

		ulong steamAppId = 1782210;

		string gamePath = SteamUtils.GetAppLocation(steamAppId, "Crab Game");

		if (gamePath == null)
		{
			Console.WriteLine("Crab Game folder was not found. Enter your Crab Game Path manually here.");
			Console.WriteLine($"({"Steam -> Right click Crab Game -> Manage -> Browse local Files".Pastel(ConsoleColor.Yellow)})");

			gamePath = Console.ReadLine();

			if (gamePath.StartsWith("\""))
				gamePath = gamePath[1..];

			if (gamePath.EndsWith("\""))
				gamePath = gamePath[..^1];
		}

		Console.WriteLine("Navigate with Arrow Keys and Enter");
		Console.WriteLine();
		Console.WriteLine();

		bool exit = false;
		while (!exit)
		{
			int initialCursorLeft = Console.CursorLeft;
			int initialCursorTop = Console.CursorTop;

			exit = MainMenu(gamePath);

			if (!exit)
			{
				Console.CursorTop++;
				Console.WriteLine("Press any key to go back...");
				Console.ReadKey(true);
			}

			int tempCursorTop = Console.CursorTop;

			for (int i = initialCursorTop; i < tempCursorTop; i++)
			{
				Console.CursorLeft = 0;
				Console.CursorTop = i;
				Console.Write(new string(' ', Console.WindowWidth));
			}

			Console.CursorTop = initialCursorTop;
			Console.CursorLeft = initialCursorLeft;
		}

	}

	private static bool MainMenu(string gamePath)
	{
		List<string> actions = new();

		bool installed = CrabCheat.GetInstalledVersion(gamePath) != null;
		bool updateAvailable = CrabCheat.CheckForUpdateAsync(gamePath).GetAwaiter().GetResult();


		if (installed)
		{
			actions.Add("Show version");
			actions.Add("Uninstall");
		}
		else
			actions.Add("Install");

		if (installed && updateAvailable)
			actions.Add("Update");

		actions.Add("Exit");

		int action = InteractiveMenu("Select an action: ", actions.ToArray());

		if (actions[action].Equals("Show version"))
		{
			Console.WriteLine("Installed version: " + CrabCheat.GetInstalledVersion(gamePath).ToString().Pastel(updateAvailable ? ConsoleColor.Red : ConsoleColor.Green));

			try
			{
				Version latest = CrabCheat.GetLatestVersion().GetAwaiter().GetResult();

				Console.WriteLine("Latest version:    " + latest.ToString().Pastel(ConsoleColor.Green));

			}
			catch (Exception)
			{
				WriteLine("Couldn't get latest version number from github. (So probably just ignore this...)", ConsoleColor.Red);
			}

			return false;
		}

		if (actions[action].Equals("Uninstall"))
		{
			Console.WriteLine($"Uninstalling CrabCheat...");
			CrabCheat.Uninstall(gamePath);
			Console.WriteLine($"{"CrabCheat".Pastel(ConsoleColor.DarkCyan)} has been uninstalled sucessfully.");

			return false;
		}

		if (actions[action].Equals("Install"))
		{
			Console.WriteLine($"Installing CrabCheat...");
			string actionDone = CrabCheat.Install(gamePath).GetAwaiter().GetResult();
			Console.WriteLine($"{"CrabCheat".Pastel(ConsoleColor.DarkCyan)} has been {actionDone} sucessfully.");

			return false;
		}

		if (actions[action].Equals("Update"))
		{
			Console.WriteLine($"Updating CrabCheat...");

			string actionDone = CrabCheat.Install(gamePath).GetAwaiter().GetResult();
			Console.WriteLine($"{"CrabCheat".Pastel(ConsoleColor.DarkCyan)} has been {actionDone} sucessfully.");

			return false;
		}

		if (action == actions.Count - 1)
		{
			return true;
		}

		return false;
	}

	private static void DrawMenu(string[] options, int selectedIndex)
	{
		int initialCursorLeft = Console.CursorLeft;
		int initialCursorTop = Console.CursorTop;
		for (int i = 0; i < options.Length; i++)
		{
			Console.CursorLeft = 0;
			Console.CursorTop = initialCursorTop + i;

			string message = " " + options[i];

			if (i == selectedIndex)
			{
				message = (">" + message).Pastel(ConsoleColor.Green);
			}
			else
				message = " " + message;

			Console.WriteLine(message);

		}

		Console.CursorTop = initialCursorTop;
		Console.CursorLeft = initialCursorLeft;
	}

	private static int InteractiveMenu(string message, string[] options, int initialIndex = 0)
	{
		bool initialCursorVisible = Console.CursorVisible;
		int initialCursorLeft = Console.CursorLeft;
		int initialCursorTop = Console.CursorTop;

		Console.CursorVisible = false;
		Console.WriteLine(message);

		int index = initialIndex;
		bool menuOpen = true;

		ConsoleKeyInfo keyInfo;

		DrawMenu(options, index);

		while (menuOpen)
		{
			keyInfo = Console.ReadKey(true);

			switch (keyInfo.Key)
			{
				case ConsoleKey.DownArrow:
					index++;

					if (index >= options.Length)
						index = 0;

					break;

				case ConsoleKey.UpArrow:
					index--;

					if (index < 0)
						index = options.Length - 1;

					break;

				case ConsoleKey.Spacebar:
				case ConsoleKey.Enter:
					menuOpen = false;
					break;
			}

			DrawMenu(options, index);

		}

		Console.CursorVisible = initialCursorVisible;
		Console.CursorTop = initialCursorTop + options.Length + 2;
		Console.CursorLeft = initialCursorLeft;

		return index;
	}

	private static void WriteLine(string message, ConsoleColor color)
	{
		Console.WriteLine(message.Pastel(color));
	}

}