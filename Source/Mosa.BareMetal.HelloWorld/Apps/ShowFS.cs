// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class ShowFS : IApp
{
	public string Name => "ShowFS";

	public string Description => "Shows information about partitions and file systems.";

	public void Execute()
	{
		Console.Write("> Finding partitions...");
		var partitions = Program.DeviceService.GetDevices<IPartitionDevice>();
		Console.WriteLine("[Completed: " + partitions.Count + " found]");

		foreach (var partition in partitions)
		{
			Console.Write("  ");
			Program.Bullet(ConsoleColor.Yellow);
			Console.Write(" ");
			Program.InBrackets(partition.Name, ConsoleColor.White, ConsoleColor.Green);
			Console.Write(" " + (partition.DeviceDriver as IPartitionDevice).BlockCount + " blocks");
			Console.WriteLine();
		}

		Console.Write("> Finding file systems...");

		foreach (var partition in partitions)
		{
			var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);
			if (!fat.IsValid) continue;

			Console.WriteLine("Found a FAT file system!");

			var location = fat.FindEntry("TEST.TXT");
			if (!location.IsValid) continue;

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
	}
}
