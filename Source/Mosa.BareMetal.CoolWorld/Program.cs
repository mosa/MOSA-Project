// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.BareMetal.CoolWorld.Console;
using Mosa.BareMetal.CoolWorld.Graphical;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.CoolWorld;

public static class Program
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
	}

	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");
		Debug.WriteLine("##PASS##");

		if (BootOptions.Contains("coolworldui") && BootOptions.GetValue("coolworldui") == "consolemode")
			ConsoleMode.Initialize();
		else
			Desktop.Start();
	}
}
