/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Mosa.Tool.Launcher
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var main = new MainForm();

			foreach (var arg in args)
			{
				switch (arg.ToLower())
				{
					case "-e": main.ExitOnLaunch = true; continue;
					case "-q": main.ExitOnLaunch = true; continue;
					case "-a": main.AutoLaunch = true; continue;
					case "-map": main.GenerateMap = true; continue;
					case "-asm": main.GenerateASM = true; continue;
					case "-qemu": main.Emulator = MainForm.VMEmulator.Qemu; continue;
					case "-vmware": main.Emulator = MainForm.VMEmulator.WMware; continue;
					case "-bochs": main.Emulator = MainForm.VMEmulator.Boches; continue;
					case "-debugger": main.MOSADebugger = true; continue;
					case "-vhd": main.DiskImage = MainForm.VMDiskFormat.VHD; continue;
					case "-img": main.DiskImage = MainForm.VMDiskFormat.IMG; continue;
					case "-vdi": main.DiskImage = MainForm.VMDiskFormat.VDI; continue;
					case "-iso": main.DiskImage = MainForm.VMDiskFormat.ISO; continue;
					default: break;
				}

				if (arg.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					main.SourceFile = arg;
				}
				else
				{
					main.SourceFile = Path.Combine(Directory.GetCurrentDirectory(), arg);
				}
			}

			Application.Run(main);
		}
	}
}