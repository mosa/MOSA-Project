// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Utility.BootImage;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Launcher
{
	public class LauncherOptions
	{
		[Option("dest")]
		public string DestinationDirectory { get; set; }

		[Option("destination-dir")]
		public string DestinationDirectoryAlt { set { DestinationDirectory = value; } }

		[Option("autostart")]
		public bool AutoStart { get; set; }

		[Option('l', "launch")]
		public bool LaunchVM { get; set; }

		[Option("launch-off")]
		public bool LaunchVMFalse { set { LaunchVM = false; } }

		[Option('e')]
		public bool ExitOnLaunch { get; set; }

		[Option('q')]
		public bool ExitOnLaunchFalse { set { ExitOnLaunch = value; } }

		[Option("emulator")]
		public EmulatorType Emulator { get; set; }

		[Option("qemu")]
		public bool EmulatorQEMU { set { Emulator = EmulatorType.Qemu; } }

		[Option("vmware")]
		public bool EmulatorVMware { set { Emulator = EmulatorType.VMware; } }

		[Option("bochs")]
		public bool EmulatorBochs { set { Emulator = EmulatorType.Bochs; } }

		[Option("image-format")]
		public ImageFormat ImageFormat { get; set; }

		[Option("vhd")]
		public bool ImageFormatVHD { set { ImageFormat = ImageFormat.VHD; } }

		[Option("img")]
		public bool ImageFormatIMG { set { ImageFormat = ImageFormat.IMG; } }

		[Option("vdi")]
		public bool ImageFormatVDI { set { ImageFormat = ImageFormat.VDI; } }

		[Option("iso")]
		public bool ImageFormatISO { set { ImageFormat = ImageFormat.ISO; } }

		[Option("vmdk")]
		public bool ImageFormatVMDK { set { ImageFormat = ImageFormat.VMDK; } }

		[Option("emulator-memory", HelpText = "Emulator memory in megabytes.")]
		public uint EmulatorMemoryInMB { get; set; }

		[Option("ssa")]
		public bool EnableSSA { get; set; }

		[Option("ir")]
		public bool EnableIROptimizations { get; set; }

		[Option("optimization-ir-off")]
		public bool IROptimizationsFalse { set { EnableIROptimizations = false; } }

		[Option("sccp")]
		public bool EnableSparseConditionalConstantPropagation { get; set; }

		[Option("optimization-sccp-off")]
		public bool EnableSparseConditionalConstantPropagationFalse { set { EnableSparseConditionalConstantPropagation = false; } }

		[Option("inline")]
		public bool EnableInlinedMethods { get; set; }

		[Option("inline-off")]
		public bool EnableInlinedMethodsFalse { set { EnableInlinedMethods = false; } }

		[Option("ir-long-expansion")]
		public bool EnableIRLongExpansion { get; set; }

		[Option("two-pass-optimizations")]
		public bool TwoPassOptimizations { get; set; }

		[Option("value-numbering")]
		public bool EnableValueNumbering { get; set; }

		[Option("value-numbering-off")]
		public bool ValueNumberingFalse { set { EnableValueNumbering = false; } }

		[Option("bit-tracker")]
		public bool EnableBitTracker { get; set; }

		[Option("bit-tracker-off")]
		public bool EnableBitTrackerFalse { set { EnableBitTracker = false; } }

		public int InlinedIRMaximum { get; set; }

		[Option("inline-level")]
		public string InlinedIRMaximumHelper { set { InlinedIRMaximum = (int)value.ParseHexOrInteger(); } }

		[Option("all-optimizations-off")]
		public bool AllOptimizationsOff
		{
			set
			{
				EnableIROptimizations = false;
				EnableInlinedMethods = false;
				TwoPassOptimizations = false;
				EnableIRLongExpansion = false;
				EnableSparseConditionalConstantPropagation = false;
				EnableValueNumbering = false;
				EnableBitTracker = false;
			}
		}

		[Option("output-nasm")]
		public bool GenerateNASMFile { get; set; }

		[Option("output-asm")]
		public bool GenerateASMFile { get; set; }

		[Option("output-map")]
		public bool GenerateMapFile { get; set; }

		[Option("output-debug")]
		public bool GenerateDebugFile { get; set; }

		[Option("output-time")]
		public bool GenerateCompileTimeFile { get; set; }

		[Option("linker-format")]
		public LinkerFormatType LinkerFormatType { get; set; }

		[Option("elf32")]
		public bool LinkerFormatTypeELF32 { set { LinkerFormatType = LinkerFormatType.Elf32; } }

		[Option("elf")]
		public bool LinkerFormatTypeELF { set { LinkerFormatType = LinkerFormatType.Elf32; } }

		public MultibootSpecification MultibootSpecification { get; set; }

		[Option("multiboot-v1")]
		public bool MultiBootV1 { set { MultibootSpecification = MultibootSpecification.V1; } }

		[Option("multiboot-v2")]
		public bool MultiBootV2 { set { MultibootSpecification = MultibootSpecification.V2; } }

		[Option("multiboot-none")]
		public bool MultiBootNone { set { MultibootSpecification = MultibootSpecification.None; } }

		[Option("no-display")]
		public bool NoDisplay { get; set; }

		[Option("platform")]
		public PlatformType PlatformType { get; set; }

		[Option("file-system")]
		public FileSystem FileSystem { get; set; }

		[Option("serial-connection")]
		public SerialConnectionOption SerialConnectionOption { get; set; }

		[Option("serial-pipe")]
		public bool SerialConnectionOptionPipe { set { SerialConnectionOption = SerialConnectionOption.Pipe; } }

		[Option("serial-tcpclient")]
		public bool SerialConnectionOptionTCPClient { set { SerialConnectionOption = SerialConnectionOption.TCPClient; } }

		[Option("serial-tcpserver")]
		public bool SerialConnectionOptionTCPServer { set { SerialConnectionOption = SerialConnectionOption.TCPServer; } }

		[Option("serial-connection-port")]
		public int SerialConnectionPort { get; set; }

		[Option("serial-connection-host")]
		public string SerialConnectionHost { get; set; }

		[Option("serial-pipe-name")]
		public string SerialPipeName { get; set; } = "MOSA";

		[Option("threading")]
		public bool EnableMultiThreading { get; set; } = true;

		[Option("threading-off")]
		public bool UseMultiThreadingCompilerFalse { set { EnableMultiThreading = false; } }

		[Option("bootloader")]
		public BootLoader BootLoader { get; set; }

		[Option("grub")]
		public bool BootLoaderGRUB { set { BootLoader = BootLoader.Grub_0_97; } }

		[Option("grub-0.97")]
		public bool BootLoaderGRUB97 { set { BootLoader = BootLoader.Grub_0_97; } }

		[Option("grub2")]
		public bool BootLoaderGRUB2 { set { BootLoader = BootLoader.Grub_2_00; } }

		[Option("syslinux")]
		public bool BootLoaderSyslinux { set { BootLoader = BootLoader.Syslinux_6_03; } }

		[Option("syslinux-6.03")]
		public bool BootLoaderSyslinux603 { set { BootLoader = BootLoader.Syslinux_6_03; } }

		[Option("syslinux-3.72")]
		public bool BootLoaderSyslinux372 { set { BootLoader = BootLoader.Syslinux_3_72; } }

		[Option("video")]
		public bool VBEVideo { get; set; }

		[Option("video-width")]
		public int Width { get; set; } = 640;

		[Option("video-height")]
		public int Height { get; set; } = 480;

		[Option("video-depth")]
		public int Depth { get; set; } = 32;

		public ulong BaseAddress { get; set; }

		[Option("base")]
		public string BaseAddressHelper { set { BaseAddress = value.ParseHexOrInteger(); } }

		[Option("emit-all-symbols")]
		public bool EmitAllSymbols { get; set; } = false;

		[Option("emit-all-symbols-false")]
		public bool EmitAllSymbolsFalse { set { EmitAllSymbols = false; } }

		[Option("emit-static-relocations")]
		public bool EmitStaticRelocations { get; set; }

		[Option("emit-relocations-false")]
		public bool EmitRelocationsFalse { set { EmitStaticRelocations = false; } }

		[Option("bootloader-image")]
		public string BootLoaderImage { get; set; }

		[Option("qemu-gdb")]
		public bool EnableQemuGDB { get; set; }

		[Option("gdb")]
		public bool LaunchGDB { get; set; }

		[Option("gdb-port")]
		public int GDBPort { get; set; }

		[Option("gdb-host")]
		public string GDBHost { get; set; }

		[Option("launch-gdb-debugger")]
		public bool LaunchGDBDebugger { get; set; }

		private string _ImageFile;

		[Option("image")]
		public string ImageFile
		{
			get
			{
				return _ImageFile;
			}
			set
			{
				_ImageFile = value;

				if (value.EndsWith(".bin"))
				{
					ImageFormat = ImageFormat.BIN;
				}
			}
		}

		[Option("debugfile")]
		public string DebugFile { get; set; }

		[Option("breakpoints")]
		public string BreakpointFile { get; set; }

		[Option("watch")]
		public string WatchFile { get; set; }

		[Option("plug-korlib")]
		public bool PlugKorlib { get; set; }

		[Option("scanner")]
		public bool EnableMethodScanner { get; set; }

		public List<IncludeFile> IncludeFiles { get; set; }

		public List<string> CompilerExtensions { get; set; }

		public List<string> Paths { get; set; }

		[Option("file", HelpText = "Path to a file which contains files to be included in the generated image file.")]
		public string IncludeFileHelper
		{
			set
			{
				if (!File.Exists(value))
				{
					Console.WriteLine("File doesn't exist \"" + value + "\"");
					return;
				}

				ReadIncludeFile(value);
			}
		}

		[Option("path")]
		public string PathHelper
		{
			set
			{
				if (!Paths.Contains(value))
				{
					Paths.Add(value);
				}
			}
		}

		public string SourceFile;

		[Value(0)]
		public string SourceFileHelper
		{
			set
			{
				if (value.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					SourceFile = value;
				}
				else
				{
					SourceFile = Path.Combine(Directory.GetCurrentDirectory(), value);
				}
			}
		}

		[Option("hunt-corlib")]
		public bool HuntForCorLib { get; set; }

		public MosaLinker.CreateExtraSectionsDelegate CreateExtraSections { get; set; }
		public MosaLinker.CreateExtraProgramHeaderDelegate CreateExtraProgramHeaders { get; set; }

		public LauncherOptions()
		{
			IncludeFiles = new List<IncludeFile>();
			Paths = new List<string>();
			DestinationDirectory = Path.Combine(Path.GetTempPath(), "MOSA");
			BootLoader = BootLoader.Syslinux_3_72; // Can't use the Default in the attribute because it would overwrite other bootloader options
			SerialConnectionOption = SerialConnectionOption.None;
			Emulator = EmulatorType.Qemu;
			ImageFormat = ImageFormat.IMG;
			LinkerFormatType = LinkerFormatType.Elf32;
			PlatformType = PlatformType.x86;
			FileSystem = FileSystem.FAT16;
			BaseAddress = 0x00400000;
			SerialConnectionHost = "127.0.0.1";
			InlinedIRMaximum = 12;
			LaunchVM = true;
			EnableIRLongExpansion = true;
			TwoPassOptimizations = true;
			EnableValueNumbering = true;
			LaunchGDB = false;
			GDBPort = 1234;
			GDBHost = "localhost";
			HuntForCorLib = true;
			EmulatorMemoryInMB = 256U;
			SerialConnectionPort = 9999;
			GenerateASMFile = false;
			EnableSSA = true;
			EnableIROptimizations = true;
			EnableSparseConditionalConstantPropagation = true;
			EnableInlinedMethods = true;
			EnableIRLongExpansion = true;
			TwoPassOptimizations = true;
			EnableValueNumbering = true;
			EnableBitTracker = true;
			MultibootSpecification = MultibootSpecification.V1;
			PlugKorlib = true;
		}

		private void ReadIncludeFile(string file)
		{
			string line;

			using (var reader = new StreamReader(file))
			{
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine();

					if (string.IsNullOrEmpty(line))
						continue;

					var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
