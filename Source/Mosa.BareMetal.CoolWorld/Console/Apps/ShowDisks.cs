// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.Disks;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class ShowDisks : IApp
{
	public string Name => "ShowDisks";

	public string Description => "Shows information about disk controllers and disks.";

	public void Execute()
	{
		System.Console.Write("> Probing for disk controllers...");
		var diskControllers = ConsoleMode.DeviceService.GetDevices<IDiskControllerDevice>();
		System.Console.WriteLine("[Completed: " + diskControllers.Count + " found]");

		foreach (var device in diskControllers)
		{
			System.Console.Write("  ");
			ConsoleMode.Bullet(ConsoleColor.Yellow);
			System.Console.Write(" ");
			ConsoleMode.InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			System.Console.WriteLine();
		}

		System.Console.Write("> Probing for disks...");
		var disks = ConsoleMode.DeviceService.GetDevices<IDiskDevice>();
		System.Console.WriteLine("[Completed: " + disks.Count + " found]");

		foreach (var disk in disks)
		{
			System.Console.Write("  ");
			ConsoleMode.Bullet(ConsoleColor.Yellow);
			System.Console.Write(" ");
			ConsoleMode.InBrackets(disk.Name, ConsoleColor.White, ConsoleColor.Green);
			System.Console.Write(" " + (disk.DeviceDriver as IDiskDevice).TotalBlocks + " blocks");
			System.Console.WriteLine();
		}
	}
}
