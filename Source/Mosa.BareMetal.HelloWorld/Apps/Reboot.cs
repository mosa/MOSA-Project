// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Reboot : IApp
{
	public string Name => "Reboot";

	public string Description => "Reboots the computer.";

	public void Execute()
	{
		Console.WriteLine("Rebooting...");

		var pc = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		pc.Reset();

		for (; ; ) HAL.Yield();
	}
}
