// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Kernel.BareMetal.x86;
using Mosa.Runtime.Plug;
using Mosa.UnitTests.Framework;
using Mosa.UnitTests.Optimization;

namespace Mosa.UnitTests.BareMetal.x86;

/// <summary>
/// Boot
/// </summary>
public static class Boot
{
	/// <summary>
	/// Main
	/// </summary>
	public static void Main()
	{
		IDT.SetInterruptHandler(ProcessInterrupt);

		UnitTestEngine.Setup(0x3F8); // Serial.COM1

		Console.SetBackground(ConsoleColor.Blue);
		Console.ClearScreen();
		Console.GotoTop();
		Console.SetForground(ConsoleColor.Yellow);
		Console.Write("MOSA OS Version 2.0 - UnitTest");
		Console.WriteLine();
		Console.WriteLine();

		UnitTestEngine.DisplayUpdate();

		UnitTestEngine.Setup(1);

		UnitTestEngine.EnterTestReadyLoop();
	}

	public static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		UnitTestEngine.Process();
	}

	private static void ForceTestCollection()
	{
		// required to force assembly to be referenced and loaded
		CommonTests.OptimizationTest1();
	}

	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootOptions.EnableDebugOutput = false;
	}
}
