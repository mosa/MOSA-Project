// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;
using Microsoft.Win32;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Configuration;

public partial class MosaSettings
{
	#region Constants

	public static class Constant
	{
		public const string MultibootVersion = "v2";

		public const int MaxErrors = 20;
		public const int ConnectionTimeOut = 15000; // in milliseconds
		public const int TimeOut = 5000; // in milliseconds
		public const int MaxAttempts = 20;
		public const int Port = 11110;
		public const int EmulatorMaxRuntime = 20; // in seconds

		public const int X86StackLocation = 0x30000;
		public const int X64StackLocation = 0x30000;
		public const int ARM32StackLocation = 0x30000;
		public const int ARM64StackLocation = 0x30000;

		public const int Optimizations_ScanWindow = 30;
		public const int Optimizations_Inline_Maximum = 12;
		public const int Optimizations_Inline_AggressiveMaximum = 24;
	}

	#endregion Constants

	public readonly Settings Settings = new();

	#region Properties

	public string AsmFile
	{
		get => Settings.GetValue(Name.CompilerDebug_AsmFile, null);
		set => Settings.SetValue(Name.CompilerDebug_AsmFile, value);
	}

	public ulong BaseAddress
	{
		get => Settings.GetValue(Name.Compiler_BaseAddress, 0ul);
		set => Settings.SetValue(Name.Compiler_BaseAddress, value);
	}

	public string BochsApp
	{
		get => Settings.GetValue(Name.AppLocation_Bochs, null);
		set => Settings.SetValue(Name.AppLocation_Bochs, value);
	}

	public string BochsBIOS
	{
		get => Settings.GetValue(Name.AppLocation_Bochs_BIOS, null);
		set => Settings.SetValue(Name.AppLocation_Bochs_BIOS, value);
	}

	public string BochsVGABIOS
	{
		get => Settings.GetValue(Name.AppLocation_Bochs_VGABIOS, null);
		set => Settings.SetValue(Name.AppLocation_Bochs_VGABIOS, value);
	}

	public string CompileTimeFile
	{
		get => Settings.GetValue(Name.CompilerDebug_CompileTimeFile, null);
		set => Settings.SetValue(Name.CompilerDebug_CompileTimeFile, value);
	}

	public string DebugFile
	{
		get => Settings.GetValue(Name.CompilerDebug_DebugFile, null);
		set => Settings.SetValue(Name.CompilerDebug_DebugFile, value);
	}

	public bool EmitBinary
	{
		get => Settings.GetValue(Name.Compiler_Binary, true);
		set => Settings.SetValue(Name.Compiler_Binary, value);
	}

	public bool EmitDwarf
	{
		get => Settings.GetValue(Name.Linker_Dwarf, false);
		set => Settings.SetValue(Name.Linker_Dwarf, value);
	}

	public string MultibootVersion
	{
		get => Settings.GetValue(Name.Multiboot_Version, Constant.MultibootVersion);
		set => Settings.SetValue(Name.Multiboot_Version, value);
	}

	public string Emulator
	{
		get => Settings.GetValue(Name.Emulator, null);
		set => Settings.SetValue(Name.Emulator, value);
	}

	public bool EmulatorDisplay
	{
		get => Settings.GetValue(Name.Emulator_Display, false);
		set => Settings.SetValue(Name.Emulator_Display, value);
	}

	public bool EmulatorGDB
	{
		get => Settings.GetValue(Name.Emulator_GDB, false);
		set => Settings.SetValue(Name.Emulator_GDB, value);
	}

	public int EmulatorMemory
	{
		get => Settings.GetValue(Name.Emulator_Memory, 128);
		set => Settings.SetValue(Name.Emulator_Memory, value);
	}

	public int EmulatorCores
	{
		get => Settings.GetValue(Name.Emulator_Cores, 1);
		set => Settings.SetValue(Name.Emulator_Cores, value);
	}

	public string EmulatorSerial
	{
		get => Settings.GetValue(Name.Emulator_Serial, null);
		set => Settings.SetValue(Name.Emulator_Serial, value);
	}

	public string EmulatorGraphics
	{
		get => Settings.GetValue(Name.Emulator_Graphics, "std");
		set => Settings.SetValue(Name.Emulator_Graphics, value);
	}

	public string EmulatorSerialHost
	{
		get => Settings.GetValue(Name.Emulator_Serial_Host, "localhost");
		set => Settings.SetValue(Name.Emulator_Serial_Host, value);
	}

	public string EmulatorSerialPipe
	{
		get => Settings.GetValue(Name.Emulator_Serial_Pipe, "MOSA");
		set => Settings.SetValue(Name.Emulator_Serial_Pipe, value);
	}

	public ushort EmulatorSerialPort
	{
		get => (ushort)Settings.GetValue(Name.Emulator_Serial_Port, 0);
		set => Settings.SetValue(Name.Emulator_Serial_Port, value);
	}

	public bool EmulatorAcceleration
	{
		get => Settings.GetValue(Name.Emulator_Acceleration, false);
		set => Settings.SetValue(Name.Emulator_Acceleration, value);
	}

	public string FileSystem
	{
		get => Settings.GetValue(Name.Image_FileSystem, null);
		set => Settings.SetValue(Name.Image_FileSystem, value);
	}

	public string GDBApp
	{
		get => Settings.GetValue(Name.AppLocation_GDB, null);
		set => Settings.SetValue(Name.AppLocation_GDB, value);
	}

	public string GDBHost
	{
		get => Settings.GetValue(Name.GDB_Host, "localhost");
		set => Settings.SetValue(Name.GDB_Host, value);
	}

	public int GDBPort
	{
		get => Settings.GetValue(Name.GDB_Port, 0);
		set => Settings.SetValue(Name.GDB_Port, value);
	}

	public string ImageFirmware
	{
		get => Settings.GetValue(Name.Image_Firmware, null);
		set => Settings.SetValue(Name.Image_Firmware, value);
	}

	public string ImageFolder
	{
		get => Settings.GetValue(Name.Image_Folder, null);
		set => Settings.SetValue(Name.Image_Folder, value);
	}

	public string DefaultFolder
	{
		get => Settings.GetValue(Name.DefaultFolder, null);
		set => Settings.SetValue(Name.DefaultFolder, value);
	}

	public string TemporaryFolder
	{
		get => Settings.GetValue(Name.TemporaryFolder, null);
		set => Settings.SetValue(Name.TemporaryFolder, value);
	}

	public string ImageFile
	{
		get => Settings.GetValue(Name.Image_ImageFile, null);
		set => Settings.SetValue(Name.Image_ImageFile, value);
	}

	public string ImageFormat
	{
		get => Settings.GetValue(Name.Image_Format, null);
		set => Settings.SetValue(Name.Image_Format, value);
	}

	public string VolumeLabel
	{
		get => Settings.GetValue(Name.Image_VolumeLabel, null);
		set => Settings.SetValue(Name.Image_VolumeLabel, value);
	}

	public int DiskBlocks
	{
		get => Settings.GetValue(Name.Image_DiskBlocks, 0);
		set => Settings.SetValue(Name.Image_DiskBlocks, value);
	}

	public string InlinedFile
	{
		get => Settings.GetValue(Name.CompilerDebug_InlinedFile, null);
		set => Settings.SetValue(Name.CompilerDebug_InlinedFile, value);
	}

	public bool LauncherExit
	{
		get => Settings.GetValue(Name.Launcher_Exit, false);
		set => Settings.SetValue(Name.Launcher_Exit, value);
	}

	public bool Launcher
	{
		get => Settings.GetValue(Name.Launcher_Launch, false);
		set => Settings.SetValue(Name.Launcher_Launch, value);
	}

	public bool LauncherStart
	{
		get => Settings.GetValue(Name.Launcher_Start, false);
		set => Settings.SetValue(Name.Launcher_Start, value);
	}

	public bool LaunchGDB
	{
		get => Settings.GetValue(Name.Launcher_GDB, false);
		set => Settings.SetValue(Name.Launcher_GDB, value);
	}

	public bool LaunchDebugger
	{
		get => Settings.GetValue(Name.Launcher_Debugger, false);
		set => Settings.SetValue(Name.Launcher_Debugger, value);
	}

	public string LinkerFormat
	{
		get => Settings.GetValue(Name.Linker_Format, null);
		set => Settings.SetValue(Name.Linker_Format, value);
	}

	public string MapFile
	{
		get => Settings.GetValue(Name.CompilerDebug_MapFile, null);
		set => Settings.SetValue(Name.CompilerDebug_MapFile, value);
	}

	public string CounterFile
	{
		get => Settings.GetValue(Name.CompilerDebug_CounterFile, null);
		set => Settings.SetValue(Name.CompilerDebug_CounterFile, value);
	}

	public string CounterFilter
	{
		get => Settings.GetValue(Name.CompilerDebug_CounterFilter, null);
		set => Settings.SetValue(Name.CompilerDebug_CounterFilter, value);
	}

	public int MaxThreads
	{
		get => Settings.GetValue(Name.Compiler_Multithreading_MaxThreads, 0);
		set => Settings.SetValue(Name.Compiler_Multithreading_MaxThreads, value);
	}

	public bool MethodScanner
	{
		get => Settings.GetValue(Name.Compiler_MethodScanner, false);
		set => Settings.SetValue(Name.Compiler_MethodScanner, value);
	}

	public string MkisofsApp
	{
		get => Settings.GetValue(Name.AppLocation_Mkisofs, null);
		set => Settings.SetValue(Name.AppLocation_Mkisofs, value);
	}

	public bool Multithreading
	{
		get => Settings.GetValue(Name.Compiler_Multithreading, true);
		set => Settings.SetValue(Name.Compiler_Multithreading, value);
	}

	public string NasmFile
	{
		get => Settings.GetValue(Name.CompilerDebug_NasmFile, null);
		set => Settings.SetValue(Name.CompilerDebug_NasmFile, value);
	}

	public string NdisasmApp

	{
		get => Settings.GetValue(Name.AppLocation_Ndisasm, null);
		set => Settings.SetValue(Name.AppLocation_Ndisasm, value);
	}

	public string OutputFile
	{
		get => Settings.GetValue(Name.Compiler_OutputFile, null);
		set => Settings.SetValue(Name.Compiler_OutputFile, value);
	}

	public string Platform
	{
		get => Settings.GetValue(Name.Compiler_Platform, "x86");
		set => Settings.SetValue(Name.Compiler_Platform, value);
	}

	public bool PlugKernel
	{
		get => Settings.GetValue(Name.Launcher_PlugKernel, false);
		set => Settings.SetValue(Name.Launcher_PlugKernel, value);
	}

	public bool PlugKorlib
	{
		get => Settings.GetValue(Name.Launcher_PlugKorlib, false);
		set => Settings.SetValue(Name.Launcher_PlugKorlib, value);
	}

	public string PostLinkHashFile
	{
		get => Settings.GetValue(Name.CompilerDebug_PostLinkHashFile, null);
		set => Settings.SetValue(Name.CompilerDebug_PostLinkHashFile, value);
	}

	public string PreLinkHashFile
	{
		get => Settings.GetValue(Name.CompilerDebug_PreLinkHashFile, null);
		set => Settings.SetValue(Name.CompilerDebug_PreLinkHashFile, value);
	}

	public string QemuX86App
	{
		get => Settings.GetValue(Name.AppLocation_QemuX86, null);
		set => Settings.SetValue(Name.AppLocation_QemuX86, value);
	}

	public string QemuX64App
	{
		get => Settings.GetValue(Name.AppLocation_QemuX64, null);
		set => Settings.SetValue(Name.AppLocation_QemuX64, value);
	}

	public string QemuARM32App
	{
		get => Settings.GetValue(Name.AppLocation_QemuARM32, null);
		set => Settings.SetValue(Name.AppLocation_QemuARM32, value);
	}

	public string QemuARM64App
	{
		get => Settings.GetValue(Name.AppLocation_QemuARM64, null);
		set => Settings.SetValue(Name.AppLocation_QemuARM64, value);
	}

	public string QemuBIOS
	{
		get => Settings.GetValue(Name.AppLocation_QemuBIOS, null);
		set => Settings.SetValue(Name.AppLocation_QemuBIOS, value);
	}

	public string QemuEdk2X86
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2X86, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2X86, value);
	}

	public string QemuEdk2X64
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2X64, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2X64, value);
	}

	public string QemuEdk2ARM32
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2ARM32, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2ARM32, value);
	}

	public string QemuEdk2ARM64
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2ARM64, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2ARM64, value);
	}

	public string QemuImgApp
	{
		get => Settings.GetValue(Name.AppLocation_QemuImg, null);
		set => Settings.SetValue(Name.AppLocation_QemuImg, value);
	}

	public bool LauncherTest
	{
		get => Settings.GetValue(Name.Launcher_Test, false);
		set => Settings.SetValue(Name.Launcher_Test, value);
	}

	public bool LauncherSerial
	{
		get => Settings.GetValue(Name.Launcher_Serial, false);
		set => Settings.SetValue(Name.Launcher_Serial, value);
	}

	public int EmulatorMaxRuntime
	{
		get => Settings.GetValue(Name.Emulator_MaxRuntime, Constant.EmulatorMaxRuntime);
		set => Settings.SetValue(Name.Emulator_MaxRuntime, Constant.EmulatorMaxRuntime);
	}

	public List<string> SearchPaths => Settings.GetValueList(Name.SearchPaths);

	public List<string> SourceFiles => Settings.GetValueList(Name.Compiler_SourceFiles);

	public string FileSystemRootInclude
	{
		get => Settings.GetValue(Name.Image_FileSystem_RootInclude, null);
		set => Settings.SetValue(Name.Image_FileSystem_RootInclude, value);
	}

	public string VmwarePlayerApp
	{
		get => Settings.GetValue(Name.AppLocation_VmwarePlayer, null);
		set => Settings.SetValue(Name.AppLocation_VmwarePlayer, value);
	}

	public string VmwareWorkstationApp
	{
		get => Settings.GetValue(Name.AppLocation_VmwareWorkstation, null);
		set => Settings.SetValue(Name.AppLocation_VmwareWorkstation, value);
	}

	public string VirtualBoxApp
	{
		get => Settings.GetValue(Name.AppLocation_VirtualBox, null);
		set => Settings.SetValue(Name.AppLocation_VirtualBox, value);
	}

	public string OSName
	{
		get => Settings.GetValue(Name.OS_Name, null);
		set => Settings.SetValue(Name.OS_Name, value);
	}

	public int MaxErrors
	{
		get => Settings.GetValue(Name.UnitTest_MaxErrors, Constant.MaxErrors);
		set => Settings.SetValue(Name.UnitTest_MaxErrors, value);
	}

	public int TimeOut
	{
		get => Settings.GetValue(Name.UnitTest_Connection_TimeOut, Constant.TimeOut);
		set => Settings.SetValue(Name.UnitTest_Connection_TimeOut, value);
	}

	public int ConnectionTimeOut
	{
		get => Settings.GetValue(Name.UnitTest_Connection_TimeOut, Constant.ConnectionTimeOut);
		set => Settings.SetValue(Name.UnitTest_Connection_TimeOut, value);
	}

	public int MaxAttempts
	{
		get => Settings.GetValue(Name.UnitTest_Connection_MaxAttempts, Constant.MaxAttempts);
		set => Settings.SetValue(Name.UnitTest_Connection_MaxAttempts, value);
	}

	public string UnitTestFilter
	{
		get => Settings.GetValue(Name.UnitTest_Filter, null);
		set => Settings.SetValue(Name.UnitTest_Filter, value);
	}

	public string ExplorerFilter
	{
		get => Settings.GetValue(Name.Explorer_Filter, null);
		set => Settings.SetValue(Name.Explorer_Filter, value);
	}

	public int TraceLevel
	{
		get => Settings.GetValue(Name.Compiler_TraceLevel, 0);
		set => Settings.SetValue(Name.Compiler_TraceLevel, value);
	}

	public bool SSA
	{
		get => Settings.GetValue(Name.Optimizations_SSA, true);
		set => Settings.SetValue(Name.Optimizations_SSA, value);
	}

	public bool BasicOptimizations
	{
		get => Settings.GetValue(Name.Optimizations_Basic, true);
		set => Settings.SetValue(Name.Optimizations_Basic, value);
	}

	public bool ValueNumbering
	{
		get => Settings.GetValue(Name.Optimizations_ValueNumbering, true);
		set => Settings.SetValue(Name.Optimizations_ValueNumbering, value);
	}

	public bool SparseConditionalConstantPropagation
	{
		get => Settings.GetValue(Name.Optimizations_SCCP, true);
		set => Settings.SetValue(Name.Optimizations_SCCP, value);
	}

	public bool Devirtualization
	{
		get => Settings.GetValue(Name.Optimizations_Devirtualization, true);
		set => Settings.SetValue(Name.Optimizations_Devirtualization, value);
	}

	public bool LoopInvariantCodeMotion
	{
		get => Settings.GetValue(Name.Optimizations_LoopInvariantCodeMotion, true);
		set => Settings.SetValue(Name.Optimizations_LoopInvariantCodeMotion, value);
	}

	public bool InlineMethods
	{
		get => Settings.GetValue(Name.Optimizations_Inline, true);
		set => Settings.SetValue(Name.Optimizations_Inline, value);
	}

	public bool InlineExplicit
	{
		get => Settings.GetValue(Name.Optimizations_Inline_Explicit, true);
		set => Settings.SetValue(Name.Optimizations_Inline_Explicit, value);
	}

	public bool LongExpansion
	{
		get => Settings.GetValue(Name.Optimizations_LongExpansion, true);
		set => Settings.SetValue(Name.Optimizations_LongExpansion, value);
	}

	public bool ReduceCodeSize
	{
		get => Settings.GetValue(Name.Optimizations_ReduceCodeSize, true);
		set => Settings.SetValue(Name.Optimizations_ReduceCodeSize, value);
	}

	public bool TwoPassOptimization
	{
		get => Settings.GetValue(Name.Optimizations_TwoPass, true);
		set => Settings.SetValue(Name.Optimizations_TwoPass, value);
	}

	public bool BitTracker
	{
		get => Settings.GetValue(Name.Optimizations_BitTracker, true);
		set => Settings.SetValue(Name.Optimizations_BitTracker, value);
	}

	public bool LoopRangeTracker
	{
		get => Settings.GetValue(Name.Optimizations_LoopRangeTracker, true);
		set => Settings.SetValue(Name.Optimizations_LoopRangeTracker, value);
	}

	public int OptimizationScanWindow
	{
		get => Settings.GetValue(Name.Optimizations_ScanWindow, Constant.Optimizations_ScanWindow);
		set => Settings.SetValue(Name.Optimizations_ScanWindow, value);
	}

	public int InlineMaximum
	{
		get => Settings.GetValue(Name.Optimizations_Inline_Maximum, Constant.Optimizations_Inline_Maximum);
		set => Settings.SetValue(Name.Optimizations_Inline_Maximum, value);
	}

	public int InlineAggressiveMaximum
	{
		get => Settings.GetValue(Name.Optimizations_Inline_AggressiveMaximum, Constant.Optimizations_Inline_AggressiveMaximum);
		set => Settings.SetValue(Name.Optimizations_Inline_AggressiveMaximum, value);
	}

	public bool PlatformOptimizations
	{
		get => Settings.GetValue(Name.Optimizations_Platform, true);
		set => Settings.SetValue(Name.Optimizations_Platform, value);
	}

	public bool EmitStatistics
	{
		get => Settings.GetValue(Name.CompilerDebug_Statistics, true);
		set => Settings.SetValue(Name.CompilerDebug_Statistics, value);
	}

	public bool EmitSumbols
	{
		get => Settings.GetValue(Name.Linker_Symbols, false);
		set => Settings.SetValue(Name.Linker_Symbols, value);
	}

	public bool EmitStaticRelocations
	{
		get => Settings.GetValue(Name.Linker_StaticRelocations, false);
		set => Settings.SetValue(Name.Linker_StaticRelocations, value);
	}

	public bool EmitShortSymbolNames
	{
		get => Settings.GetValue(Name.Linker_ShortSymbolNames, false);
		set => Settings.SetValue(Name.Linker_ShortSymbolNames, value);
	}

	public List<string> InlineAggressive
	{
		get => Settings.GetValueList(Name.Optimizations_Inline_Aggressive);
	}

	public List<string> InlineExclude
	{
		get => Settings.GetValueList(Name.Optimizations_Inline_Exclude);
	}

	public bool FullCheckMode
	{
		get => Settings.GetValue(Name.CompilerDebug_FullCheckMode, false);
		set => Settings.SetValue(Name.CompilerDebug_FullCheckMode, value);
	}

	public string BreakpointFile
	{
		get => Settings.GetValue(Name.Debugger_BreakpointFile, null);
		set => Settings.SetValue(Name.Debugger_BreakpointFile, value);
	}

	public string WatchFile
	{
		get => Settings.GetValue(Name.Debugger_WatchFile, null);
		set => Settings.SetValue(Name.Debugger_WatchFile, value);
	}

	public bool ExplorerStart
	{
		get => Settings.GetValue(Name.Explorer_Start, false);
		set => Settings.SetValue(Name.Explorer_Start, value);
	}

	public bool DebugDiagnostic
	{
		get => Settings.GetValue(Name.Explorer_DebugDiagnostic, false);
		set => Settings.SetValue(Name.Explorer_DebugDiagnostic, value);
	}

	public bool MultibootVideo
	{
		get => Settings.GetValue(Name.Multiboot_Video, false);
		set => Settings.SetValue(Name.Multiboot_Video, value);
	}

	public int MultibootVideoWidth
	{
		get => Settings.GetValue(Name.Multiboot_Video_Width, 640);
		set => Settings.SetValue(Name.Multiboot_Video_Width, value);
	}

	public int MultibootVideoHeight
	{
		get => Settings.GetValue(Name.Multiboot_Video_Height, 480);
		set => Settings.SetValue(Name.Multiboot_Video_Height, value);
	}

	public string GraphwizApp
	{
		get => Settings.GetValue(Name.AppLocation_Graphwiz, null);
		set => Settings.SetValue(Name.AppLocation_Graphwiz, value);
	}

	public string OSBootOptions
	{
		get => Settings.GetValue(Name.OS_BootOptions, null);
		set => Settings.SetValue(Name.OS_BootOptions, value);
	}

	public int BootLoaderTimeout
	{
		get => Settings.GetValue(Name.BootLoaderTimeout, 0);
		set => Settings.SetValue(Name.BootLoaderTimeout, value);
	}

	public int InitialStackLocation
	{
		get => Settings.GetValue(Name.Compiler_InitialStackAddress, 0);
		set => Settings.SetValue(Name.Compiler_InitialStackAddress, value);
	}

	#endregion Properties

	public MosaSettings()
	{
	}

	public MosaSettings(Settings settings)
	{
		Merge(settings);
	}

	public MosaSettings(MosaSettings mosaSettings)
	{
		Merge(mosaSettings);
	}

	public void Merge(Settings settings)
	{
		Settings.Merge(settings);
	}

	public void Merge(MosaSettings mosaSettings)
	{
		Settings.Merge(mosaSettings.Settings);
	}

	public void LoadAppLocations()
	{
		AppLocationsSettings.GetAppLocations(this);
	}

	public void LoadArguments(string[] args)
	{
		var settings = Import.RecursiveReader(CommandLineArguments.Map, args);

		Settings.Merge(settings);
	}

	public void SetDefaultSettings()
	{
		TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");

		MethodScanner = false;
		Multithreading = true;
		Platform = "%DEFAULT%";
		Multithreading = true;
		BaseAddress = 0x00400000;
		EmitBinary = true;
		TraceLevel = 0;

		ImageFile = "%DEFAULT%";

		DebugFile = null;
		AsmFile = null;
		MapFile = null;
		CounterFile = null;
		NasmFile = null;
		InlinedFile = null;

		MethodScanner = false;

		SSA = true;
		BasicOptimizations = true;
		ValueNumbering = true;
		SparseConditionalConstantPropagation = true;
		Devirtualization = true;
		BitTracker = true;
		LoopRangeTracker = true;
		LoopInvariantCodeMotion = true;
		LongExpansion = true;
		TwoPassOptimization = true;
		PlatformOptimizations = true;
		InlineMethods = true;
		InlineExplicit = true;
		InlineMaximum = Constant.Optimizations_Inline_Maximum;
		InlineAggressiveMaximum = Constant.Optimizations_Inline_AggressiveMaximum;
		OptimizationScanWindow = Constant.Optimizations_ScanWindow;
		ReduceCodeSize = false;

		Emulator = "Qemu";
		EmulatorDisplay = false;
		EmulatorCores = 1;

		EmulatorSerial = "TCPServer";
		EmulatorSerialHost = "127.0.0.1";
		EmulatorSerialPort = Constant.Port;
		EmulatorSerialPipe = "MOSA";

		MultibootVersion = Constant.MultibootVersion;

		ImageFirmware = "bios";
		ImageFolder = TemporaryFolder;
		ImageFormat = "img";
		FileSystem = "fat16";
		ImageFile = "%DEFAULT%";

		OSName = "MOSA";
		VolumeLabel = "MOSA";
		DiskBlocks = 0;

		MaxErrors = Constant.MaxErrors;
		ConnectionTimeOut = Constant.ConnectionTimeOut;
		MaxAttempts = Constant.MaxAttempts;

		PlugKernel = true;
		PlugKorlib = true;

		LauncherStart = false;
		LauncherExit = false;
		Launcher = false;

		MultibootVideo = false;
		MultibootVideoWidth = 640;
		MultibootVideoHeight = 480;

		EmitSumbols = false;
		EmitStaticRelocations = false;
		EmitShortSymbolNames = false;

		LinkerFormat = "elf32";

		ExplorerFilter = "%REGISTRY%";

		InitialStackLocation = 0;
	}

	public void NormalizeSettings()
	{
		ImageFormat = ToLower(ImageFormat);
		FileSystem = ToLower(FileSystem);
		EmulatorSerial = ToLower(EmulatorSerial);
		Emulator = ToLower(Emulator);
		Platform = ToLower(Platform);
		LinkerFormat = ToLower(LinkerFormat);

		switch (Platform.ToLowerInvariant())
		{
			case "x86": Platform = "x86"; break;
			case "x64": Platform = "x64"; break;
			case "arm32": Platform = "ARM32"; break;
			case "arm64": Platform = "ARM64"; break;
		}
	}

	public void AdjustSettings()
	{
		if (OptimizationScanWindow < 0)
			OptimizationScanWindow = 0;

		if (InlineMaximum < 0)
			InlineMaximum = 0;

		if (InlineAggressiveMaximum < 0)
			InlineAggressiveMaximum = 0;

		VolumeLabel ??= OSName;
	}

	public void ResolveDefaults()
	{
		AdjustSettings();

		if (string.IsNullOrWhiteSpace(TemporaryFolder) || TemporaryFolder == "%DEFAULT%")
		{
			TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");
		}

		if (string.IsNullOrWhiteSpace(ImageFolder) || ImageFolder == "%DEFAULT%")
		{
			ImageFolder = TemporaryFolder;
		}

		if (string.IsNullOrWhiteSpace(DefaultFolder) || DefaultFolder == "%DEFAULT%")
		{
			DefaultFolder = TemporaryFolder;
		}

		if (ExplorerFilter == "%REGISTRY%")
		{
			ExplorerFilter = GetRegistry(WindowsRegistry.ExplorerFilter, string.Empty);
		}

		if (Platform == "%REGISTRY%")
		{
			Platform = GetRegistry(WindowsRegistry.ExplorerPlatform, string.Empty);
		}

		if (string.IsNullOrWhiteSpace(Platform) || Platform == "%DEFAULT%")
		{
			Platform = "x86";
		}
	}

	public void ResolveFileAndPathSettings()
	{
		string baseFilename;

		if (OutputFile != null && OutputFile != "%DEFAULT%")
		{
			baseFilename = Path.GetFileNameWithoutExtension(OutputFile);
		}
		else if (SourceFiles != null && SourceFiles.Count != 0)
		{
			baseFilename = Path.GetFileNameWithoutExtension(SourceFiles[0]);
		}
		else if (ImageFile != null && ImageFile != "%DEFAULT%")
		{
			baseFilename = Path.GetFileNameWithoutExtension(ImageFile);
		}
		else
		{
			baseFilename = "_mosa_";
		}

		if (OutputFile is null or "%DEFAULT%")
		{
			OutputFile = Path.Combine(DefaultFolder, $"{baseFilename}.bin");
		}

		if (ImageFile == "%DEFAULT%")
		{
			ImageFile = Path.Combine(ImageFolder, $"{baseFilename}.{ImageFormat}");
		}

		if (MapFile == "%DEFAULT%")
		{
			MapFile = Path.Combine(DefaultFolder, $"{baseFilename}-map.txt");
		}

		if (CounterFile == "%DEFAULT%")
		{
			CounterFile = Path.Combine(DefaultFolder, $"{baseFilename}-counters.txt");
		}

		if (CompileTimeFile == "%DEFAULT%")
		{
			CompileTimeFile = Path.Combine(DefaultFolder, $"{baseFilename}-time.txt");
		}

		if (DebugFile == "%DEFAULT%")
		{
			DebugFile = Path.Combine(DefaultFolder, $"{baseFilename}.debug");
		}

		if (InlinedFile == "%DEFAULT%")
		{
			InlinedFile = Path.Combine(DefaultFolder, $"{baseFilename}-inlined.txt");
		}

		if (PreLinkHashFile == "%DEFAULT%")
		{
			PreLinkHashFile = Path.Combine(DefaultFolder, $"{baseFilename}-prelink-hash.txt");
		}

		if (PostLinkHashFile == "%DEFAULT%")
		{
			PostLinkHashFile = Path.Combine(DefaultFolder, $"{baseFilename}-postlink-hash.txt");
		}

		if (AsmFile == "%DEFAULT%")
		{
			AsmFile = Path.Combine(DefaultFolder, $"{baseFilename}.asm");
		}

		if (NasmFile == "%DEFAULT%")
		{
			NasmFile = Path.Combine(DefaultFolder, $"{baseFilename}.nasm");
		}

		if (InitialStackLocation == 0)
		{
			switch (Platform.ToLowerInvariant())
			{
				case "x86": InitialStackLocation = Constant.X86StackLocation; break;
				case "x64": InitialStackLocation = Constant.X64StackLocation; break;
				case "arm32": InitialStackLocation = Constant.ARM32StackLocation; break;
				case "arm64": InitialStackLocation = Constant.ARM64StackLocation; break;
			}
		}
	}

	public void AddStandardPlugs()
	{
		if (PlugKorlib)
		{
			AddSourceFile("Mosa.Plug.Korlib.dll");
			AddSourceFile($"Mosa.Plug.Korlib.{Platform}.dll");
		}

		if (PlugKernel)
		{
			AddSourceFile($"Mosa.Kernel.BareMetal.{Platform}.dll");
		}
	}

	public void ExpandSearchPaths()
	{
		if (SourceFiles == null)
			return;

		foreach (var sourcefile in SourceFiles)
		{
			var full = Path.GetFullPath(sourcefile);
			var path = Path.GetDirectoryName(full);

			if (!string.IsNullOrWhiteSpace(path))
			{
				AddSearchPath(path);
			}
		}
	}

	public static string GetRegistry(string name, string defaultValue)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			try
			{
				return (string)Registry.CurrentUser
						.OpenSubKey(WindowsRegistry.Software)?
						.OpenSubKey(WindowsRegistry.MosaApp)?
						.GetValue(name, defaultValue) ?? defaultValue;
			}
			catch
			{
				return defaultValue;
			}
		}

		return defaultValue;
	}

	public static void SetRegistry(string name, string value)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			Registry.CurrentUser
			.OpenSubKey(WindowsRegistry.Software)
			.OpenSubKey(WindowsRegistry.MosaApp, RegistryKeyPermissionCheck.ReadWriteSubTree)
			.SetValue(name, value);
		}
	}

	private string ToLower(string value)
	{
		if (value == null)
			return string.Empty;

		if (value == "%DEFAULT%" || value == "%REGISTRY%")
			return value;

		return value.ToLowerInvariant().Trim();
	}

	#region Custom Helpers

	public void ClearSourceFiles()
	{
		Settings.ClearProperty(Name.Compiler_SourceFiles);
	}

	public void AddSourceFile(string filename)
	{
		Settings.AddPropertyListValue(Name.Compiler_SourceFiles, filename);
	}

	public void ClearSearchPaths()
	{
		Settings.ClearProperty(Name.SearchPaths);
	}

	public void AddSearchPath(string filename)
	{
		Settings.AddPropertyListValue(Name.SearchPaths, filename);
	}

	#endregion Custom Helpers
}
