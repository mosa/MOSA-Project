// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.x86;
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

		Screen.Color = 0x0;
		Screen.Clear();
		Screen.GotoTop();
		Screen.Color = 0x0E; // ScreenColor.Yellow;
		Screen.Write("MOSA OS Version 1.6 - UnitTest");
		Screen.NextLine();
		Screen.NextLine();

		UnitTestQueue.Setup();
		UnitTestRunner.Setup();

		UnitTestRunner.EnterTestReadyLoop();
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
}
