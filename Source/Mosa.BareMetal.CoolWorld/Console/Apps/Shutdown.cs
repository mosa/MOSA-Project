// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Services;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Shutdown : IApp
{
	public string Name => "Shutdown";

	public string Description => "Shuts down the computer.";

	public void Execute()
	{
		System.Console.WriteLine("Shutting down...");

		var pc = Kernel.BareMetal.Kernel.ServiceManager.GetFirstService<PCService>();
		pc.Shutdown();

		for (; ; ) HAL.Yield();
	}
}
