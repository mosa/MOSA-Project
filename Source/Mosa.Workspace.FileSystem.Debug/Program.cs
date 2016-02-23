// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using Mosa.Workspace.FileSystem.Debug.Synthetic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mosa.Workspace.FileSystem.Debug
{
	internal class Program
	{
		private delegate void Test();

		protected static byte[] GetResource(string name)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var stream = assembly.GetManifestResourceStream("Mosa.Workspace.FileSystem.Debug." + name);
			var binary = new BinaryReader(stream);
			return binary.ReadBytes((int)stream.Length);
		}

		private static void Main(string[] args)
		{
			// Create synthetic ram disk device
			var ramDiskDevice = new RamDiskDevice(1024 * 1024 * 10 / 512);

			// Create master boot block record
			var mbr = new MasterBootBlock(ramDiskDevice);
			mbr.DiskSignature = 0x12345678;
			mbr.Partitions[0].Bootable = true;
			mbr.Partitions[0].StartLBA = 17;
			mbr.Partitions[0].TotalBlocks = ramDiskDevice.TotalBlocks - 17;
			mbr.Partitions[0].PartitionType = PartitionType.FAT12;
			mbr.Write();

			// Create partition device
			var partitionDevice = new PartitionDevice(ramDiskDevice, mbr.Partitions[0], false);

			// Set FAT settings
			var fatSettings = new FatSettings();

			fatSettings.FATType = FatType.FAT12;
			fatSettings.FloppyMedia = false;
			fatSettings.VolumeLabel = "MOSADISK";
			fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };

			// Create FAT file system
			var fat = new FatFileSystem(partitionDevice);
			fat.Format(fatSettings);

			if (fat.IsValid)
			{
				switch (fat.FATType)
				{
					case FatType.FAT12: Console.WriteLine("FAT12"); break;
					case FatType.FAT16: Console.WriteLine("FAT16"); break;
					case FatType.FAT32: Console.WriteLine("FAT32"); break;
					default: Console.WriteLine("Unknown"); break;
				}
				Console.WriteLine("  Volume Name: " + fat.VolumeLabel);
			}
			else
			{
				Console.WriteLine("Unknown File System");
			}

			var files = new List<IncludeFile>();

			files.Add(new IncludeFile("CREDITS.TXT", GetResource("CREDITS.txt")));
			files.Add(new IncludeFile("LICENSE.TXT", GetResource("LICENSE.txt")));

			foreach (var includeFile in files)
			{
				Console.WriteLine("Writing File: " + includeFile.Filename);

				var fileAttributes = new FatFileAttributes();
				if (includeFile.Archive) fileAttributes |= FatFileAttributes.Archive;
				if (includeFile.ReadOnly) fileAttributes |= FatFileAttributes.ReadOnly;
				if (includeFile.Hidden) fileAttributes |= FatFileAttributes.Hidden;
				if (includeFile.System) fileAttributes |= FatFileAttributes.System;

				string newname = (Path.GetFileNameWithoutExtension(includeFile.Filename).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Filename).PadRight(4).Substring(1, 3)).ToUpper();
				var location = fat.CreateFile(newname, fileAttributes);

				if (!location.IsValid)
					throw new Exception("Unable to write file");

				var fatFileStream = new FatFileStream(fat, location);
				fatFileStream.Write(includeFile.Content, 0, includeFile.Content.Length);
				fatFileStream.Flush();

				Console.WriteLine("  Source Length: " + includeFile.Content.Length.ToString());
				Console.WriteLine("  Stream Length: " + fatFileStream.Length.ToString());
			}

			foreach (var includeFile in files)
			{
				Console.WriteLine("Searching File: " + includeFile.Filename);

				var location = fat.FindEntry(includeFile.Filename);

				if (location.IsValid)
				{
					Console.WriteLine("  Found: " + includeFile.Filename);

					var fatFileStream = new FatFileStream(fat, location);
					Console.WriteLine("  Length: " + fatFileStream.Length.ToString());

					for (;;)
					{
						int i = fatFileStream.ReadByte();

						if (i < 0)
							break;

						Console.Write((char)i);
					}
					Console.WriteLine();
					fatFileStream.Position = 0;

					var buffer = new byte[fatFileStream.Length];
					fatFileStream.Read(buffer, 0, (int)fatFileStream.Length);

					Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
				}
				else
				{
					Console.WriteLine("  Not Found: " + includeFile.Filename);
				}
			}

			return;
		}
	}
}
