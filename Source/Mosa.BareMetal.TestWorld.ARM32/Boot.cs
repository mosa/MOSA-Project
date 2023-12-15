// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Optimization;

namespace Mosa.BareMetal.TestWorld.ARM32;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");

		Console.BackgroundColor = ConsoleColor.Blue;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();
		Console.WriteLine("Mosa.BareMetal.TextWorld.ARM32");
		Console.WriteLine();

		Division.DivisionBy7(254u);

		// Will never get here!
		Debug.WriteLine("ERROR: Thread Start Failure");
		Debug.Fatal();
	}

	private static int counter = 0;

	public static void ForceInclude()
	{
		Mosa.Kernel.BareMetal.ARM32.PlatformPlug.ForceInclude();
	}
}
