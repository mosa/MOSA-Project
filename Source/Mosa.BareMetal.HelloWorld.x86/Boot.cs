// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceSystem.Service;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.BareMetal;
using Mosa.Kernel.BareMetal.x86;

namespace Mosa.BareMetal.HelloWorld.x86;

/// <summary>
/// Boot
/// </summary>
public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");

		var deviceService = Mosa.Kernel.BareMetal.Kernel.ServiceManger.GetFirstService<DeviceService>();

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();
		Console.Write("> Probing for ISA devices...");

		var isaDevices = deviceService.GetChildrenOf(deviceService.GetFirstDevice<ISABus>());
		Console.WriteLine("[Completed: " + isaDevices.Count + " found]");

		foreach (var device in isaDevices)
		{
			Console.Write("  ");
			Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.WriteLine();
		}

		Console.Write("> Probing for PCI devices...");
		var devices = deviceService.GetDevices<PCIDevice>();
		Console.WriteLine("[Completed: " + devices.Count + " found]");

		foreach (var device in devices)
		{
			Console.Write("  ");
			Bullet(ConsoleColor.Yellow);
			Console.Write(" ");

			var pciDevice = device.DeviceDriver as PCIDevice;
			InBrackets(device.Name + ": " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubSystemVendorID.ToString("x") + " (" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + ")", ConsoleColor.White, ConsoleColor.Green);

			var children = deviceService.GetChildrenOf(device);

			if (children.Count != 0)
			{
				var child = children[0];

				Console.WriteLine();
				Console.Write("    ");

				var pciDevice2 = child.DeviceDriver as PCIDevice;
				InBrackets(child.Name, ConsoleColor.White, ConsoleColor.Green);
			}

			Console.WriteLine();
		}

		Console.Write("> Probing for disk controllers...");
		var diskcontrollers = deviceService.GetDevices<IDiskControllerDevice>();
		Console.WriteLine("[Completed: " + diskcontrollers.Count + " found]");

		foreach (var device in diskcontrollers)
		{
			Console.Write("  ");
			Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			InBrackets(device.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.WriteLine();
		}

		Console.Write("> Probing for disks...");
		var disks = deviceService.GetDevices<IDiskDevice>();
		Console.WriteLine("[Completed: " + disks.Count + " found]");

		foreach (var disk in disks)
		{
			Console.Write("  ");
			Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			InBrackets(disk.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.Write(" " + (disk.DeviceDriver as IDiskDevice).TotalBlocks + " blocks");
			Console.WriteLine();
		}

		var partitionService = Mosa.Kernel.BareMetal.Kernel.ServiceManger.GetFirstService<PartitionService>();

		partitionService.CreatePartitionDevices();

		Console.Write("> Finding partitions...");
		var partitions = deviceService.GetDevices<IPartitionDevice>();
		Console.WriteLine("[Completed: " + partitions.Count + " found]");

		foreach (var partition in partitions)
		{
			Console.Write("  ");
			Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			InBrackets(partition.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.Write(" " + (partition.DeviceDriver as IPartitionDevice).BlockCount.ToString() + " blocks");
			Console.WriteLine();
		}

		//Console.Write("> Finding file systems...");

		//foreach (var partition in partitions)
		//{
		//	var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);

		//	if (fat.IsValid)
		//	{
		//		Console.WriteLine("Found a FAT file system!");

		//		const string filename = "TEST.TXT";

		//		var location = fat.FindEntry(filename);

		//		if (location.IsValid)
		//		{
		//			Console.Write("Found: " + filename);

		//			var fatFileStream = new FatFileStream(fat, location);

		//			uint len = (uint)fatFileStream.Length;

		//			Console.WriteLine(" - Length: " + len + " bytes");

		//			Console.Write("Reading File: ");

		//			for (; ; )
		//			{
		//				int i = fatFileStream.ReadByte();

		//				if (i < 0)
		//					break;

		//				Console.Write((char)i);
		//			}

		//			Console.WriteLine();
		//		}
		//	}
		//}

		while (true)
		{ }
	}

	//[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootOptions.EnableDebugOutput = true;
		//BootOptions.EnableVirtualMemory = true;
		//BootOptions.EnableMinimalBoot = true;
	}

	public static void InBrackets(string message, ConsoleColor outerColor, ConsoleColor innerColor)
	{
		var restore = Console.ForegroundColor;
		Console.ForegroundColor = outerColor;
		Console.Write("[");
		Console.ForegroundColor = innerColor;
		Console.Write(message);
		Console.ForegroundColor = outerColor;
		Console.Write("]");
		Console.ForegroundColor = restore;
	}

	public static void Bullet(ConsoleColor color)
	{
		var restore = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write("*");
		Console.ForegroundColor = restore;
	}

	public static void Include()
	{
		Mosa.Kernel.BareMetal.x86.Scheduler.SwitchToThread(null);
	}
}
