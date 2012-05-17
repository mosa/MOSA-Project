/*
* (c) 2012 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using Mosa.Utility.BootImage;

namespace Mosa.Tool.CreateBootImage
{
	/// <summary>
	/// 
	/// </summary>
	class Program
	{

		public static Options Parse(string filename)
		{
			Options options = new Options();

			StreamReader reader = File.OpenText(filename);

			while (true)
			{
				string line = reader.ReadLine();
				if (line == null) break;

				if (string.IsNullOrEmpty(line))
					continue;

				string[] parts = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

				switch (parts[0].Trim())
				{
					case "-mbr": options.MBROption = true; options.MBRCode = (parts.Length > 1) ? File.ReadAllBytes(parts[1]) : null; break;
					case "-boot": options.FatBootCode = (parts.Length > 1) ? File.ReadAllBytes(parts[1]) : null; break;
					case "-vhd": options.ImageFormat = ImageFormatType.VHD; break;
					case "-img": options.ImageFormat = ImageFormatType.IMG; break;
					case "-vdi": options.ImageFormat = ImageFormatType.VDI; break;
					case "-syslinux": options.PatchSyslinuxOption = true; break;
					case "-guid": if (parts.Length > 1) options.MediaGuid = new Guid(parts[1]); break;
					case "-snapguid": if (parts.Length > 1) options.MediaLastSnapGuid = new Guid(parts[1]); break;
					case "-fat12": options.FileSystem = FileSystemType.FAT12; break;
					case "-fat16": options.FileSystem = FileSystemType.FAT16; break;
					case "-fat32": options.FileSystem = FileSystemType.FAT32; break;
					case "-file": if (parts.Length > 2) options.IncludeFiles.Add(new IncludeFile(parts[1], parts[2]));
						else options.IncludeFiles.Add(new IncludeFile(parts[1])); break;
					case "-blocks": options.BlockCount = Convert.ToUInt32(parts[1]); break;
					case "-volume": options.VolumeLabel = parts[1]; break;
					default: break;
				}
			}

			reader.Close();

			return options;

		}

		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("MakeImageBoot v1.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2012. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			bool valid = args.Length == 2;

			if (valid)
				valid = System.IO.File.Exists(args[0]);

			if (!valid)
			{
				Console.WriteLine("Usage: CreateBootImage <boot.config file> <image name>");
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			Console.WriteLine("Building image...");

			try
			{
				Options options = Parse(args[0]);

				if (options == null)
				{
					Console.WriteLine("Usage: CreateBootImage <boot.config file> <image name>");
					Console.Error.WriteLine("ERROR: Invalid options");
					return -1;
				}

				options.DiskImageFileName = args[1];

				Generator.Create(options);

				Console.WriteLine("Completed!");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("ERROR: " + e.ToString());
				return -1;
			}

			return 0;
		}

	}
}
