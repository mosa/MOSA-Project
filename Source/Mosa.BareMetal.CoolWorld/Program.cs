// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.BareMetal.CoolWorld.Console;
using Mosa.BareMetal.CoolWorld.Graphical;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.CoolWorld;

public static class Program
{
	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");
		Debug.WriteLine("##PASS##");

		if (BootOptions.Contains("consolemode"))
			ConsoleMode.Initialize();
		else
			Desktop.Start();
	}
}
