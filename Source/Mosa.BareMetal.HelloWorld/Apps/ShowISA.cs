// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceDriver.ISA;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class ShowISA : IApp
{
	public string Name => "ShowISA";

	public string Description => "Shows information about ISA devices.";

	public void Execute()
	{
		Console.Write("> Probing for ISA devices...");

		var isaDevices = Program.DeviceService.GetChildrenOf(Program.DeviceService.GetFirstDevice<ISABus>());
		Console.WriteLine("[Completed: " + isaDevices.Count + " found]");

		foreach (var device in isaDevices)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			Program.InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.WriteLine();
		}
	}
}
