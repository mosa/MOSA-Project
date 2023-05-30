﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;

namespace Mosa.Demo.Application;

/// <summary>
/// Shell
/// </summary>
public class Shell : BaseApplication, IConsoleApp
{
	public override int Start(string parameters)
	{
		Console.WriteLine();

		Console.WriteLine("[MOSA Command Shell Ready]");

		Console.WriteLine();

		bool exit = false;

		while (!exit)
		{
			Console.Write("> ");

			var line = Console.ReadLine().Trim();

			if (string.IsNullOrEmpty(line))
			{
				Console.WriteLine();
				continue;
			}

			int index = line.IndexOf(' ');

			string cmd = index <= 0 ? line : line.Substring(0, index);

			var app = AppFactory(cmd);

			if (app == null)
			{
				Console.WriteLine("Unknown command!");
				continue;
			}

			string param = string.Empty;

			if (index > 0)
			{
				int len = line.Length + 1 - index;

				if (len > 0)
				{
					param = cmd.Substring(index + 1, len);
				}
			}

			AppManager.StartApp(app, Console.Input, Console.Output, Console.EnableEcho, param);
		}

		return 0;
	}

	public static IConsoleApp AppFactory(string name)
	{
		return name switch
		{
			"clear" => new Clear(),
			"mem" => new Mem(),
			"showpci" => new ShowPCI(),
			"credits" => new Credits(),
			"shutdown" => new Shutdown(),
			"reboot" => new Reboot(),
			"test" => new Test(),
			"test2" => new Test(),
			"test3" => new Test(),
			_ => null
		};
	}
}
