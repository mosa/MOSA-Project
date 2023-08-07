// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceSystem.Service;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld;

public static class Program
{
	public static void EntryPoint()
	{
		Debug.WriteLine("Program::EntryPoint()");

		var deviceService = Kernel.BareMetal.Kernel.ServiceManger.GetFirstService<DeviceService>();

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

				InBrackets(child.Name, ConsoleColor.White, ConsoleColor.Green);
			}

			Console.WriteLine();
		}

		Console.Write("> Probing for disk controllers...");
		var diskControllers = deviceService.GetDevices<IDiskControllerDevice>();
		Console.WriteLine("[Completed: " + diskControllers.Count + " found]");

		foreach (var device in diskControllers)
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
			Console.Write(" " + (partition.DeviceDriver as IPartitionDevice).BlockCount + " blocks");
			Console.WriteLine();
		}

		Console.Write("> Finding file systems...");

		foreach (var partition in partitions)
		{
			var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);
			if (!fat.IsValid)
				continue;

			Console.WriteLine("Found a FAT file system!");

			var location = fat.FindEntry("TEST.TXT");
			if (location.IsValid)
			{
				Console.Write("Found test file!");

				var testStream = new FatFileStream(fat, location);

				Console.WriteLine(" - Length: " + (uint)testStream.Length + " bytes");
				Console.Write("Reading File: ");

				for (; ; )
				{
					var i = testStream.ReadByte();

					if (i < 0)
						break;

					Console.Write((char)i);
				}

				Console.WriteLine();
			}

			var bmpLoc = fat.FindEntry("WALLP.BMP");
			if (!bmpLoc.IsValid)
				continue;

			Console.Write("Found wallpaper file! ");

			var wallpaperStream = new FatFileStream(fat, bmpLoc);

			Console.WriteLine(" - Length: " + (uint)wallpaperStream.Length + " bytes");
			Console.WriteLine();
		}

		Debug.WriteLine("##PASS##");

		AppManager.Execute("shell");
	}

	//[Plug("Mosa.Runtime.StartUp::BootOptions")]
	public static void SetBootOptions()
	{
		BootOptions.EnableDebugOutput = true;
		//BootOptions.EnableVirtualMemory = true;
		//BootOptions.EnableMinimalBoot = true;
	}

	private static void InBrackets(string message, ConsoleColor outerColor, ConsoleColor innerColor)
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

	private static void Bullet(ConsoleColor color)
	{
		var restore = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write("*");
		Console.ForegroundColor = restore;
	}
}
