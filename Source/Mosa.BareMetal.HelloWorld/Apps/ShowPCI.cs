// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.PCI;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class ShowPCI : IApp
{
	public string Name => "ShowPCI";

	public string Description => "Shows information about PCI devices.";

	public void Execute()
	{
		Console.Write("> Probing for PCI devices...");
		var devices = Program.DeviceService.GetAllDevices(DeviceBusType.PCI);
		Console.WriteLine("[Completed: " + devices.Count + " found]");

		foreach (var device in devices)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");

			var pciDevice = (PCIDevice)device.Parent.DeviceDriver;
			var name = device.DeviceDriver == null ? device.DeviceDriverRegistryEntry.Name : device.Name;

			Program.InBrackets(pciDevice.Device.Name + ": " + name + " " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubSystemVendorID.ToString("x") + " (" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + ")", ConsoleColor.White, ConsoleColor.Green);
			Console.WriteLine();
		}
	}
}
