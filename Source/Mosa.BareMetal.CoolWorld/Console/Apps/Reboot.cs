// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Reboot : IApp
{
	public string Name => "Reboot";

	public string Description => "Reboots the computer.";

	public void Execute()
	{
		System.Console.WriteLine("Rebooting...");

		var pc = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		var success = pc.Reset();

		if (!success) System.Console.WriteLine("Error while trying to reboot.");

		for (; ; ) HAL.Yield();
	}
}
