// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Demo.CoolWorld.x86.Mosa.AppSystem;

namespace Mosa.Demo.CoolWorld.x86.Mosa.Application
{
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

				if (String.IsNullOrEmpty(line))
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

				string param = String.Empty;

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
			switch (name)
			{
				case "clear": return new Clear();
				case "mem": return new Mem();
				case "showpci": return new ShowPCI();
				case "credits": return new Credits();
				case "shutdown": return new Shutdown();
				case "reboot": return new Reboot();
				default: return null;
			}
		}
	}
}
