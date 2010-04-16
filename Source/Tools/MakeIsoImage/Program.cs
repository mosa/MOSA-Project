/*
 * Copyright (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Author(s):
 *  Royce Mitchell III (royce3) <royce3 [at] gmail [dot] com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mosa.Tools.MakeIsoImage
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
#if false
            var test = new Mosa.MakeIsoImage.Iso9660Generator(false);
            test.AddFile("Long File Name.txt",new System.IO.FileInfo("C:\\cvs\\mosa\\Mosa\\Tools\\MakeIsoImage\\bin\\Debug\\Long File Name.txt"));
            test.Generate("Iso9660Generator.iso");
            return;
#endif

			try {
				Iso9660Generator iso = new Iso9660Generator(false);
				int i;

				for (i = 0; i < args.Length; i++) {
					if (args[i].Trim()[0] != '-')
						break;
					switch (args[i].Trim()) {
						case "-boot":
							i++;
							iso.AddBootFile(args[i], new System.IO.FileInfo(args[i]));
							break;
						case "-boot-load-size":
							short bootLoadSize;
							if (short.TryParse(args[++i], out bootLoadSize))
								iso.BootLoadSize(bootLoadSize);
							break;
						case "-boot-info-table":
							iso.SetBootInfoTable(true);
							break;
						case "-label":
							i++;
							iso.SetVolumeLabel(args[i]);
							break;
						default:
							break;
					}
				}

				// at this point, args[i] should be our iso image name
				if (i >= args.Length) {
					Console.Error.Write("Missing iso file name");
					return -1;
				}

				string isoFileName = args[i++];

				// now args[i] is root folder
				if (i >= args.Length) {
					Console.Error.Write("Missing root folder");
					return -1;
				}

				while (i < args.Length)
					AddDirectoryTree(iso, args[i++], "");

				iso.Generate(isoFileName);

				Console.WriteLine("Completed!");
			}

			catch (Exception e) {
				Console.Error.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		static private void AddDirectoryTree(Iso9660Generator iso, string root, string virtualPrepend)
		{
			if(Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.Win32NT)
				root = root.Replace('/', '\\');
			
			DirectoryInfo dirinfo = new DirectoryInfo(root);

			foreach (FileInfo file in dirinfo.GetFiles())
				iso.AddFile(virtualPrepend + file.Name, file);

			foreach (DirectoryInfo dir in dirinfo.GetDirectories()) {
				iso.MkDir(virtualPrepend + dir.Name);
				AddDirectoryTree(iso, root + '/' + dir.Name, virtualPrepend + dir.Name + '/');
			}
		}
	}
}
