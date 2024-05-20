// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Shell : IApp
{
	public string Name => "Shell";

	public string Description => "Runs the interactive shell.";

	public void Execute()
	{
		System.Console.WriteLine();
		System.Console.WriteLine("MOSA Shell v2.4");
		System.Console.WriteLine("Enter \"quit\" to exit the shell.");

		while (true)
		{
			System.Console.Write("> ");

			var cmd = System.Console.ReadLine();

			if (cmd == "quit")
				break;

			if (!AppManager.Execute(cmd))
			{
				System.Console.WriteLine("Unknown command: " + cmd);
			}
		}
	}
}
