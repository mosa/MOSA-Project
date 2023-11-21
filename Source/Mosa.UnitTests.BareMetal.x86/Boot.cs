// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Kernel.BareMetal.Intel;
using Mosa.Kernel.BareMetal.x86;
using Mosa.Runtime.Plug;
using Mosa.UnitTests.Optimization;

namespace Mosa.UnitTests.BareMetal.x86;

public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = false;
	}

	public static void Main()
	{
		IDT.SetInterruptHandler(ProcessInterrupt);

		UnitTestEngine.Setup(SerialController.COM1);
		UnitTestEngine.EnterTestReadyLoop();
	}

	private static void ProcessInterrupt(uint interrupt, uint errorCode) => UnitTestEngine.Process();

	private static void ForceLoad()
	{
		Division.DivisionBy7(1);
	}
}
