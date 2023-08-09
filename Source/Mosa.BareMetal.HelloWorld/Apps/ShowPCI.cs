// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.PCI;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class ShowPCI : IApp
{
	public string Name => "ShowPCI";

	public string Description => "Shows information about PCI devices.";

	public void Execute()
	{
		Console.Write("> Probing for PCI devices...");
		var devices = Program.DeviceService.GetDevices<PCIDevice>();
		Console.WriteLine("[Completed: " + devices.Count + " found]");

		foreach (var device in devices)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");

			var pciDevice = device.DeviceDriver as PCIDevice;
			Program.InBrackets(device.Name + ": " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubSystemVendorID.ToString("x") + " (" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + ")", ConsoleColor.White, ConsoleColor.Green);

			var children = Program.DeviceService.GetChildrenOf(device);

			if (children.Count != 0)
			{
				var child = children[0];

				Console.WriteLine();
				Console.Write("    ");

				Program.InBrackets(child.Name, ConsoleColor.White, ConsoleColor.Green);
			}

			Console.WriteLine();
		}
	}
}
