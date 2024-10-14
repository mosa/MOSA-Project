// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.Disks;
using Mosa.FileSystem.FAT;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class ShowFS : IApp
{
	public string Name => "ShowFS";

	public string Description => "Shows information about partitions and file systems.";

	public void Execute()
	{
		System.Console.Write("> Finding partitions...");
		var partitions = ConsoleMode.DeviceService.GetDevices<IPartitionDevice>();
		System.Console.WriteLine("[Completed: " + partitions.Count + " found]");

		foreach (var partition in partitions)
		{
			System.Console.Write("  ");
			ConsoleMode.Bullet(ConsoleColor.Yellow);
			System.Console.Write(" ");
			ConsoleMode.InBrackets(partition.Name, ConsoleColor.White, ConsoleColor.Green);
			System.Console.Write(" " + (partition.DeviceDriver as IPartitionDevice).BlockCount + " blocks");
			System.Console.WriteLine();
		}

		System.Console.Write("> Finding file systems...");

		foreach (var partition in partitions)
		{
			var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);
			if (!fat.IsValid) continue;

			System.Console.WriteLine("Found a FAT file system!");

			var location = fat.FindEntry("TEST.TXT");
			if (!location.IsValid) continue;

			System.Console.Write("Found test file!");

			var testStream = new FatFileStream(fat, location);

			System.Console.WriteLine(" - Length: " + (uint)testStream.Length + " bytes");
			System.Console.Write("Reading File: ");

			for (; ; )
			{
				var i = testStream.ReadByte();

				if (i < 0)
					break;

				System.Console.Write((char)i);
			}

			System.Console.WriteLine();
		}
	}
}
