// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Shell : IApp
{
	public string Name => "Shell";

	public string Description => "Runs the interactive shell.";

	public void Execute()
	{
		Console.WriteLine();
		Console.WriteLine("MOSA Shell v2.4");
		Console.WriteLine("Enter \"quit\" to exit the shell.");

		while (true)
		{
			Console.Write("> ");

			var cmd = Console.ReadLine().ToLower();

			if (cmd == "quit")
				break;

			if (!AppManager.Execute(cmd))
				Console.WriteLine("Unknown command: " + cmd);
		}
	}
}
