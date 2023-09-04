// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal;
using Mosa.Kernel.BareMetal.x86;
using Mosa.Runtime.Plug;
using Serial = Mosa.Kernel.BareMetal.x86.Serial;

namespace Mosa.UnitTests.BareMetal.x86;

public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = false;
		//BootSettings.EnableVirtualMemory = true;
		//BootSettings.EnableMinimalBoot = true;
	}

	public static void Main()
	{
		IDT.SetInterruptHandler(ProcessInterrupt);

		UnitTestEngine.Setup(Serial.COM1);
		UnitTestEngine.EnterTestReadyLoop();
	}

	private static void ProcessInterrupt(uint interrupt, uint errorCode) => UnitTestEngine.Process();
}
