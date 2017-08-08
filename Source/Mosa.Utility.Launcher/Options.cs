// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class Options
	{
		[Option("destination-dir")]
		public string DestinationDirectory { get; set; }

		[Option("dest")]
		public string DestinationDirectoryExtraOption
		{
			set { DestinationDirectory = value; }
		}

		[Option('a')]
		public bool AutoStart { get; set; }

		[Option('l', "launch", Default = true)]
		public bool LaunchEmulator { get; set; }

		[Option("launch-off")]
		public bool LaunchEmulatorExtraOption
		{
			set { LaunchEmulator = false; }
		}

		[Option('e')]
		public bool ExitOnLaunch { get; set; }

		[Option('q')]
		public bool ExitOnLaunchExtraOption
		{
			set { ExitOnLaunch = value; }
		}

		[Option("emulator")]
		public EmulatorType Emulator { get; set; }

		[Option("qemu")]
		public bool EmulatorQEMU
		{
			set { Emulator = EmulatorType.Qemu; }
		}

		[Option("vmware")]
		public bool EmulatorVMware
		{
			set { Emulator = EmulatorType.VMware; }
		}

		[Option("bochs")]
		public bool EmulatorBochs
		{
			set { Emulator = EmulatorType.Bochs; }
		}

		[Option("image-format")]
		public ImageFormat ImageFormat { get; set; }

		[Option("vhd")]
		public bool ImageFormatVHD
		{
			set { ImageFormat = ImageFormat.VHD; }
		}

		[Option("img")]
		public bool ImageFormatIMG
		{
			set { ImageFormat = ImageFormat.IMG; }
		}

		[Option("vdi")]
		public bool ImageFormatVDI
		{
			set { ImageFormat = ImageFormat.VDI; }
		}

		[Option("iso")]
		public bool ImageFormatISO
		{
			set { ImageFormat = ImageFormat.ISO; }
		}

		[Option("vmdk")]
		public bool ImageFormatVMDK
		{
			set { ImageFormat = ImageFormat.VMDK; }
		}

		[Option("emulator-memory", Default = 128U, HelpText = "Emulator memory in megabytes.")]
		public uint EmulatorMemoryInMB { get; set; }

		[Option("ssa", Default = true)]
		public bool EnableSSA { get; set; }

		[Option("ir", Default = true)]
		public bool EnableIROptimizations { get; set; }

		[Option("optimization-ir-off")]
		public bool IROptimizationsExtraOption
		{
			set { EnableIROptimizations = false; }
		}

		[Option("sccp", Default = true)]
		public bool EnableSparseConditionalConstantPropagation { get; set; }

		[Option("optimization-sccp-off")]
		public bool EnableSCCPExtraOption
		{
			set { EnableSparseConditionalConstantPropagation = false; }
		}

		[Option("inline", Default = true)]
		public bool EnableInlinedMethods { get; set; }

		[Option("inline-off")]
		public bool EnableInlinedMethodsExtraOption
		{
			set { EnableInlinedMethods = false; }
		}

		public int InlinedIRMaximum { get; set; }

		[Option("inline-level", Default = "8")]
		public string InlinedIRMaximumHelper
		{
			set { InlinedIRMaximum = (int)value.ParseHexOrDecimal(); }
		}

		[Option("nasm")]
		public bool GenerateNASMFile { get; set; }

		[Option("asm", Default = false)]
		public bool GenerateASMFile { get; set; }

		[Option("map")]
		public bool GenerateMapFile { get; set; }

		[Option("debuginfo", Default = false)]
		public bool GenerateDebugFile { get; set; }

		[Option("linker-format")]
		public LinkerFormatType LinkerFormatType { get; set; }

		[Option("elf32")]
		public bool LinkerFormatTypeELF32
		{
			set { LinkerFormatType = LinkerFormatType.Elf32; }
		}

		[Option("elf")]
		public bool LinkerFormatTypeELF
		{
			set { LinkerFormatType = LinkerFormatType.Elf32; }
		}

		[Option("boot-format")]
		public BootFormat BootFormat { get; set; }

		[Option("mb0.7")]
		public bool BootFormatMultiboot07
		{
			set { BootFormat = BootFormat.Multiboot_0_7; }
		}

		[Option("platform")]
		public PlatformType PlatformType { get; set; }

		[Option("file-system")]
		public FileSystem FileSystem { get; set; }

		[Option("debug-connection")]
		public DebugConnectionOption DebugConnectionOption { get; set; }

		[Option("pipe")]
		public bool DebugConnectionOptionPipe
		{
			set { DebugConnectionOption = DebugConnectionOption.Pipe; }
		}

		[Option("tcpclient")]
		public bool DebugConnectionOptionTCPClient
		{
			set { DebugConnectionOption = DebugConnectionOption.TCPClient; }
		}

		[Option("tcpserver")]
		public bool DebugConnectionOptionTCPServer
		{
			set { DebugConnectionOption = DebugConnectionOption.TCPServer; }
		}

		[Option("debug-connection-port", Default = 9999)]
		public int DebugConnectionPort { get; set; }

		[Option("debug-connection-address", Default = "127.0.0.1")]
		public string DebugConnectionAddress { get; set; }

		[Option("debug-pipe-name", Default = "MOSA")]
		public string DebugPipeName { get; set; }

		[Option("threading", Default = true)]
		public bool UseMultipleThreadCompiler { get; set; }

		[Option("threading-off")]
		public bool UseMultipleThreadCompilerExtraOption
		{
			set { UseMultipleThreadCompiler = false; }
		}

		[Option("bootloader")]
		public BootLoader BootLoader { get; set; }

		[Option("grub")]
		public bool BootLoaderGRUB
		{
			set { BootLoader = BootLoader.Grub_0_97; }
		}

		[Option("grub-0.97")]
		public bool BootLoaderGRUB97
		{
			set { BootLoader = BootLoader.Grub_0_97; }
		}

		[Option("grub-2")]
		public bool BootLoaderGRUB2
		{
			set { BootLoader = BootLoader.Grub_2_00; }
		}

		[Option("grub2")]
		public bool BootLoaderGRUB2_2
		{
			set { BootLoader = BootLoader.Grub_2_00; }
		}

		[Option("syslinux")]
		public bool BootLoaderSyslinux
		{
			set { BootLoader = BootLoader.Syslinux_6_03; }
		}

		[Option("syslinux-6.03")]
		public bool BootLoaderSyslinux603
		{
			set { BootLoader = BootLoader.Syslinux_6_03; }
		}

		[Option("syslinux-3.72")]
		public bool BootLoaderSyslinux372
		{
			set { BootLoader = BootLoader.Syslinux_3_72; }
		}

		[Option("video", Default = false)]
		public bool VBEVideo { get; set; }

		[Option("video-width", Default = 640)]
		public int Width { get; set; }

		[Option("video-height", Default = 480)]
		public int Height { get; set; }

		[Option("video-depth", Default = 32)]
		public int Depth { get; set; }

		public ulong BaseAddress { get; set; }

		[Option("base", Default = "0x00400000")]
		public string BaseAddressHelper
		{
			set { BaseAddress = value.ParseHexOrDecimal(); }
		}

		[Option("symbols", Default = false)]
		public bool EmitSymbols { get; set; }

		[Option("symbols-false")]
		public bool EmitSymbolsExtraOption
		{
			set { EmitSymbols = false; }
		}

		[Option("relocations", Default = false)]
		public bool EmitRelocations { get; set; }

		[Option("relocations-false")]
		public bool EmitRelocationsExtraOption
		{
			set { EmitRelocations = false; }
		}

		[Option("x86-irq-methods", Default = true)]
		public bool Emitx86IRQMethods { get; set; }

		[Option("x86-irq-methods-false")]
		public bool Emitx86IRQMethodsExtraOption
		{
			set { Emitx86IRQMethods = false; }
		}

		[Option("bootloader-image", Default = null)]
		public string BootLoaderImage { get; set; }

		[Option("qemu-gdb", Default = false)]
		public bool EnableQemuGDB { get; set; }

		[Option("gdb", Default = false)]
		public bool LaunchGDB { get; set; }

		[Option("launch-mosa-debugger", Default = false)]
		public bool LaunchMosaDebugger { get; set; }

		public List<IncludeFile> IncludeFiles { get; set; }

		[Option("file", HelpText = "Path to a file which contains files to be included in the generated image file.")]
		public string IncludeFilePath
		{
			set
			{
				if(!File.Exists(value))
				{
					Console.WriteLine("File doesn't exist \"" + value + "\"");
					return;
				}

				AppendIncludeFiles(value);
			}
		}

		private string _sourceFile;
		[Value(0)]
		public string SourceFile
		{
			get { return _sourceFile; }
			set
			{
				if (value.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					_sourceFile = value;
				}
				else
				{
					_sourceFile = Path.Combine(Directory.GetCurrentDirectory(), value);
				}
			}
		}

		public Options()
		{
			IncludeFiles = new List<IncludeFile>();
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			BootLoader = BootLoader.Syslinux_3_72; //Can't use the Default in the attribute because it would overwrite other bootloader options
			BootFormat = BootFormat.Multiboot_0_7;
			DebugConnectionOption = DebugConnectionOption.None;
			Emulator = EmulatorType.Qemu;
			ImageFormat = ImageFormat.IMG;
			LinkerFormatType = LinkerFormatType.Elf32;
			PlatformType = PlatformType.X86;
			FileSystem = FileSystem.FAT16;
		}

		private void AppendIncludeFiles(string file)
		{
			string line;
			using (StreamReader reader = new StreamReader(file))
			{
				while(!reader.EndOfStream)
				{
					line = reader.ReadLine();

					if (string.IsNullOrEmpty(line))
						continue;

					string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (parts.Length == 0)
						continue;

					if (!File.Exists(parts[0]))
					{
						Console.WriteLine("File not found \"" + parts[0] + "\"");
						continue;
					}

					if (parts.Length > 1)
					{
						IncludeFiles.Add(new IncludeFile(parts[0], parts[1]));
					}
					else
					{
						IncludeFiles.Add(new IncludeFile(parts[0]));
					}
				}
			}
		}
	}
}
