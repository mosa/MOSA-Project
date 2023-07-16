// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.UnitTests.Optimization;

namespace Mosa.UnitTests.x86;

/// <summary>
/// Boot
/// </summary>
public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
	public static void SetInitialMemory()
	{
		KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
	}

	/// <summary>
	/// Main
	/// </summary>
	public static void Main()
	{
		Setup();

		EnterTestReadyLoop();
	}

	private static void Setup()
	{
		IDT.SetInterruptHandler(null);
		Panic.Setup();
		UnitTestEngine.Setup(Serial.COM1);

		// Initialize interrupts
		PIC.Setup();
		IDT.Setup();
		GDT.Setup();
	}

	public static void EnterTestReadyLoop()
	{
		Screen.Color = 0x0;
		Screen.Clear();
		Screen.GotoTop();
		Screen.Color = ScreenColor.Yellow;
		Screen.WriteLine("MOSA OS Version 2.4");
		Screen.NextLine();

		Screen.WriteLine("[UnitTest]");

		Setup();

		UnitTestEngine.EnterTestReadyLoop();
	}

	private static void ForceTestCollection()
	{
		// required to force assembly to be referenced and loaded
		CommonTests.OptimizationTest1();
	}
}
