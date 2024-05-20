// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.Framework;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class ShowISA : IApp
{
	public string Name => "ShowISA";

	public string Description => "Shows information about ISA devices.";

	public void Execute()
	{
		System.Console.Write("> Probing for ISA devices...");

		var isaDevices = ConsoleMode.DeviceService.GetAllDevices(DeviceBusType.ISA);
		System.Console.WriteLine("[Completed: " + isaDevices.Count + " found]");

		foreach (var device in isaDevices)
		{
			System.Console.Write("  ");
			ConsoleMode.Bullet(ConsoleColor.Yellow);
			System.Console.Write(" ");
			ConsoleMode.InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			System.Console.WriteLine();
		}
	}
}
