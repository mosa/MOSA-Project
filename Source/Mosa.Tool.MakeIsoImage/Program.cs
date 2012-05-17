/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

using System;
using Mosa.Utility.IsoImage;

namespace Mosa.Tool.MakeIsoImage
{
	internal class Program
	{
		/// <summary>
		/// Mains the specified args.
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		private static int Main(string[] args)
		{
			Console.WriteLine("MakeIsoImage v0.9 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Royce Mitchell III (royce3@gmail.com)");
			Console.WriteLine();

			// TODO FIXME - support remappings something like -map boot/boot.bin=c:/muos/build/debug/bin/iso9660_boot.bin

			try
			{
				Options options = new Options();

				int i;

				for (i = 0; i < args.Length; i++)
				{
					if (args[i].Trim()[0] != '-')
						break;

					switch (args[i].Trim())
					{
						case "-boot":
//							iso.AddBootFile(args[i], new System.IO.FileInfo(args[i]));
							options.BootFileName = args[++i];
							break;
						case "-boot-load-size":
							short bootLoadSize;
							if (short.TryParse(args[++i], out bootLoadSize))
								options.BootLoadSize = bootLoadSize;
							break;
						case "-boot-info-table":
							options.BootInfoTable = true;
							break;
						case "-label":
							options.VolumeLabel = args[++i];
							break;
						default:
							break;
					}
				}

				// at this point, args[i] should be our iso image name
				if (i >= args.Length)
				{
					Console.Error.Write("Missing iso file name");
					return -1;
				}

				options.IsoFileName = args[i++];

				// now args[i] is root folder
				if (i >= args.Length)
				{
					Console.Error.Write("Missing root folder");
					return -1;
				}

				while (i < args.Length)
					options.IncludeFiles.Add(args[i++]);

				Iso9660Generator iso = new Iso9660Generator(options);
				iso.Generate();

				Console.WriteLine("Completed!");
			}

			catch (Exception e)
			{
				Console.Error.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

	}
}
