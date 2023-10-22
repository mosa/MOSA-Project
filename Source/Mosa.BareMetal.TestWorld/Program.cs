// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime.Plug;

namespace Mosa.BareMetal.TestWorld;

public static class Program
{
	[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootSettings.EnableDebugOutput = true;
	}

	public static void EntryPoint()
	{
		Debug.WriteLine("Program::Main()");
		Debug.WriteLine("##PASS##");

		for (; ; )
			HAL.Yield();
	}
}
