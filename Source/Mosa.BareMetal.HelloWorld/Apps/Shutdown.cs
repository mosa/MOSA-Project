// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Shutdown : IApp
{
	public string Name => "Shutdown";

	public string Description => "Shuts down the computer.";

	public void Execute()
	{
		Console.WriteLine("Shutting down...");

		var pc = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		pc.Shutdown();

		for (;;) HAL.Yield();
	}
}
