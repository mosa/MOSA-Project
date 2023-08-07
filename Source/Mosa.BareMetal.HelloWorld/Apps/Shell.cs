// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Shell : IApp
{
	public string Name => "Shell";

	public void Execute()
	{
		Console.WriteLine();
		Console.WriteLine("MOSA Shell v2.4");

		while (true)
		{
			Console.Write("> ");

			var cmd = Console.ReadLine().ToLower();

			if (!AppManager.Execute(cmd))
				Console.WriteLine("Unknown command: " + cmd);
		}
	}
}
