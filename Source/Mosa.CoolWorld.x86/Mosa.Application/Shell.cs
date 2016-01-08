// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;

namespace Mosa.Application
{
	/// <summary>
	///
	/// </summary>
	public class Shell : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			Console.WriteLine();

			Console.WriteLine("[MOSA Shell Ready]");

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

				line = line.Trim();

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
			switch (name)
			{
				case "mem": return new Mem();
				case "credits": return new Credits();
				default: return null;
			}
		}
	}
}
