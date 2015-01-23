/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Utility.Launcher
{
	public class Options
	{
		public string SourceFile { get; set; }

		public string DestinationDirectory { get; set; }

		public bool ExitOnLaunch { get; set; }

		public bool AutoLaunch { get; set; }

		public EmulatorType Emulator { get; set; }

		public bool MOSADebugger { get; set; }

		public ImageFormat ImageFormat { get; set; }

		public uint MemoryInMB { get; set; }

		public bool EnableSSA { get; set; }

		public bool EnableIROptimizations { get; set; }

		public bool EnablePromoteTemporaryVariables { get; set; }

		public bool EnableSparseConditionalConstantPropagation { get; set; }

		public bool GenerateASMFile { get; set; }

		public bool GenerateMapFile { get; set; }

		public LinkerFormat LinkerFormat { get; set; }

		public BootFormat BootFormat { get; set; }

		public PlatformType PlatformType { get; set; }

		public FileSystemFormat FileSystemFormat { get; set; }

		public DebugConnectionOption DebugConnectionOption { get; set; }

		public Options()
		{
			EnableSSA = true;
			EnableIROptimizations = true;
			EnablePromoteTemporaryVariables = true;
			EnableSparseConditionalConstantPropagation = true;
			Emulator = EmulatorType.Qemu;
			ImageFormat = ImageFormat.IMG;
			BootFormat = BootFormat.Multiboot_0_7;
			PlatformType = PlatformType.X86;
			LinkerFormat = Launcher.LinkerFormat.Elf32;
			MemoryInMB = 128;
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			FileSystemFormat = FileSystemFormat.FAT16;
			DebugConnectionOption = DebugConnectionOption.None;
		}

		public void LoadArguments(string[] args)
		{
			foreach (var arg in args)
			{
				switch (arg.ToLower())
				{
					case "-e": ExitOnLaunch = true; continue;
					case "-q": ExitOnLaunch = true; continue;
					case "-a": AutoLaunch = true; continue;
					case "-map": GenerateMapFile = true; continue;
					case "-asm": GenerateASMFile = true; continue;
					case "-qemu": Emulator = EmulatorType.Qemu; continue;
					case "-vmware": Emulator = EmulatorType.WMware; continue;
					case "-bochs": Emulator = EmulatorType.Boches; continue;
					case "-debugger": MOSADebugger = true; continue;
					case "-vhd": ImageFormat = ImageFormat.VHD; continue;
					case "-img": ImageFormat = ImageFormat.IMG; continue;
					case "-vdi": ImageFormat = ImageFormat.VDI; continue;
					case "-iso": ImageFormat = ImageFormat.ISO; continue;
					case "-vmdk": ImageFormat = ImageFormat.VMDK; continue;
					case "-elf32": LinkerFormat = LinkerFormat.Elf32; continue;
					case "-elf": LinkerFormat = LinkerFormat.Elf32; continue;
					case "-pe32": LinkerFormat = LinkerFormat.PE32; continue;
					case "-pe": LinkerFormat = LinkerFormat.PE32; continue;
					case "multibootHeader-0.7": BootFormat = BootFormat.Multiboot_0_7; continue;
					case "mb0.7": BootFormat = BootFormat.Multiboot_0_7; continue;
					case "pipe": DebugConnectionOption = DebugConnectionOption.Pipe; continue;
					case "tcpclient": DebugConnectionOption = DebugConnectionOption.TCPClient; continue;
					case "tcpserver": DebugConnectionOption = DebugConnectionOption.TCPServer; continue;
					default: break;
				}

				if (arg.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					SourceFile = arg;
				}
				else
				{
					SourceFile = Path.Combine(Directory.GetCurrentDirectory(), arg);
				}
			}
		}
	}
}
