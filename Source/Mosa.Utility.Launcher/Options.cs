// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Utility.BootImage;
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

		public bool EnableVariablePromotion { get; set; }

		public bool EnableSparseConditionalConstantPropagation { get; set; }

		public bool EnableInlinedMethods { get; set; }

		public int InlinedIRMaximum { get; set; }

		public bool GenerateASMFile { get; set; }

		public bool GenerateMapFile { get; set; }

		public LinkerFormatType LinkerFormatType { get; set; }

		public BootFormat BootFormat { get; set; }

		public PlatformType PlatformType { get; set; }

		public FileSystem FileSystem { get; set; }

		public DebugConnectionOption DebugConnectionOption { get; set; }

		public int DebugConnectionPort { get; set; }

		public string DebugConnectionAddress { get; set; }

		public string DebugPipeName { get; set; }

		public bool UseMultipleThreadCompiler { get; set; }

		public BootLoader BootLoader { get; set; }
		public bool VBEVideo { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Depth { get; set; }

		public ulong BaseAddress { get; set; }
		public bool EmitSymbols { get; set; }
		public bool EmitRelocations { get; set; }

		public bool Emitx86IRQMethods { get; set; }

		public Options()
		{
			EnableSSA = true;
			EnableIROptimizations = true;
			EnableVariablePromotion = true;
			EnableSparseConditionalConstantPropagation = true;
			Emulator = EmulatorType.Qemu;
			ImageFormat = ImageFormat.IMG;
			BootFormat = BootFormat.Multiboot_0_7;
			PlatformType = PlatformType.X86;
			LinkerFormatType = LinkerFormatType.Elf32;
			MemoryInMB = 128;
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			FileSystem = FileSystem.FAT16;
			DebugConnectionOption = DebugConnectionOption.None;
			DebugConnectionPort = 9999;
			DebugConnectionAddress = "127.0.0.1";
			DebugPipeName = "MOSA";
			UseMultipleThreadCompiler = true;
			EnableInlinedMethods = true;
			InlinedIRMaximum = 8;
			BootLoader = BootLoader.Syslinux_3_72;
			VBEVideo = false;
			Width = 640;
			Height = 480;
			Depth = 32;
			BaseAddress = 0x00400000;
			EmitRelocations = false;
			EmitSymbols = false;
			Emitx86IRQMethods = true;
		}

		public void LoadArguments(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				switch (arg.ToLower())
				{
					case "-e": ExitOnLaunch = true; continue;
					case "-q": ExitOnLaunch = true; continue;
					case "-a": AutoLaunch = true; continue;
					case "-map": GenerateMapFile = true; continue;
					case "-asm": GenerateASMFile = true; continue;
					case "-qemu": Emulator = EmulatorType.Qemu; continue;
					case "-vmware": Emulator = EmulatorType.VMware; continue;
					case "-bochs": Emulator = EmulatorType.Bochs; continue;
					case "-debugger": MOSADebugger = true; continue;
					case "-vhd": ImageFormat = ImageFormat.VHD; continue;
					case "-img": ImageFormat = ImageFormat.IMG; continue;
					case "-vdi": ImageFormat = ImageFormat.VDI; continue;
					case "-iso": ImageFormat = ImageFormat.ISO; continue;
					case "-vmdk": ImageFormat = ImageFormat.VMDK; continue;
					case "-elf32": LinkerFormatType = LinkerFormatType.Elf32; continue;
					case "-elf": LinkerFormatType = LinkerFormatType.Elf32; continue;
					case "-mb0.7": BootFormat = BootFormat.Multiboot_0_7; continue;
					case "-pipe": DebugConnectionOption = DebugConnectionOption.Pipe; continue;
					case "-tcpclient": DebugConnectionOption = DebugConnectionOption.TCPClient; continue;
					case "-tcpserver": DebugConnectionOption = DebugConnectionOption.TCPServer; continue;
					case "-grub": BootLoader = BootLoader.Grub_0_97; continue;
					case "-grub-0.97": BootLoader = BootLoader.Grub_0_97; continue;
					case "-grub-2": BootLoader = BootLoader.Grub_2_00; continue;
					case "-grub2": BootLoader = BootLoader.Grub_2_00; continue;
					case "-syslinux": BootLoader = BootLoader.Syslinux_6_03; continue;
					case "-syslinux-6.03": BootLoader = BootLoader.Syslinux_6_03; continue;
					case "-syslinux-3.72": BootLoader = BootLoader.Syslinux_3_72; continue;
					case "-inline": EnableInlinedMethods = true; continue;
					case "-inline-off": EnableInlinedMethods = false; continue;
					case "-threading-off": UseMultipleThreadCompiler = false; continue;
					case "-video": VBEVideo = true; continue;
					case "-base": BaseAddress = args[++i].ParseHexOrDecimal(); continue;
					case "-symbols": EmitSymbols = true; continue;
					case "-symbols-false": EmitSymbols = false; continue;
					case "-relocations": EmitRelocations = true; continue;
					case "-relocations-false": EmitRelocations = false; continue;
					case "-x86-irq-methods": Emitx86IRQMethods = true; continue;
					case "-x86-irq-methods-false": Emitx86IRQMethods = false; continue;
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
