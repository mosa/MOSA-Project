// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class ShowDisks : IApp
{
	public string Name => "ShowDisks";

	public string Description => "Shows information about disk controllers and disks.";

	public void Execute()
	{
		Console.Write("> Probing for disk controllers...");
		var diskControllers = Program.DeviceService.GetDevices<IDiskControllerDevice>();
		Console.WriteLine("[Completed: " + diskControllers.Count + " found]");

		foreach (var device in diskControllers)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			Program.InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.WriteLine();
		}

		Console.Write("> Probing for disks...");
		var disks = Program.DeviceService.GetDevices<IDiskDevice>();
		Console.WriteLine("[Completed: " + disks.Count + " found]");

		foreach (var disk in disks)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			Program.InBrackets(disk.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.Write(" " + (disk.DeviceDriver as IDiskDevice).TotalBlocks + " blocks");
			Console.WriteLine();
		}
	}
}
