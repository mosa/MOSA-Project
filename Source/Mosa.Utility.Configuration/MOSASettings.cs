// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Configuration;

public class MosaSettings
{
	#region Constants

	public static class Constant
	{
		public const string MultibootVersion = "v1";

		public const int MaxErrors = 1000;
		public const int ConnectionTimeOut = 10000; // in milliseconds
		public const int TimeOut = 10000; // in milliseconds
		public const int MaxAttempts = 20;
		public const int Port = 11110;
	}

	#endregion Constants

	public Settings Settings { get; } = new Settings();

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
		set => Settings.SetValue(Name.Compiler_Binary, true);
	}

	public bool EmitDwarf
	{
		get => Settings.GetValue(Name.Linker_Dwarf, false);
		set => Settings.SetValue(Name.Linker_Dwarf, false);
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

	public string EmulatorSVGA
	{
		get => Settings.GetValue(Name.Emulator_SVGA, "std");
		set => Settings.SetValue(Name.Emulator_SVGA, value);
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

	public string QEMUApp
	{
		get => Settings.GetValue(Name.AppLocation_Qemu, null);
		set => Settings.SetValue(Name.AppLocation_Qemu, value);
	}

	public string QEMUBios
	{
		get => Settings.GetValue(Name.AppLocation_QemuBIOS, null);
		set => Settings.SetValue(Name.AppLocation_QemuBIOS, value);
	}

	public string QEMUEdk2X86
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2X86, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2X86, value);
	}

	public string QEMUEdk2X64
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2X64, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2X64, value);
	}

	public string QEMUEdk2ARM
	{
		get => Settings.GetValue(Name.AppLocation_QemuEDK2ARM, null);
		set => Settings.SetValue(Name.AppLocation_QemuEDK2ARM, value);
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
		get => Settings.GetValue(Name.Emulator_MaxRuntime, 10);
		set => Settings.SetValue(Name.Emulator_MaxRuntime, 10);
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

	public int OptimizationBasicWindow
	{
		get => Settings.GetValue(Name.Optimizations_Basic_Window, 5);
		set => Settings.SetValue(Name.Optimizations_Basic_Window, value);
	}

	public int InlineMaximum
	{
		get => Settings.GetValue(Name.Optimizations_Inline_Maximum, 12);
		set => Settings.SetValue(Name.Optimizations_Inline_Maximum, value);
	}

	public int InlineAggressiveMaximum
	{
		get => Settings.GetValue(Name.Optimizations_Inline_AggressiveMaximum, 24);
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

	public bool EmitInline
	{
		get => Settings.GetValue(Name.Compiler_EmitInline, false);
		set => Settings.SetValue(Name.Compiler_EmitInline, value);
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

	public int MultibootVideoDepth
	{
		get => Settings.GetValue(Name.Multiboot_Video_Depth, 32);
		set => Settings.SetValue(Name.Multiboot_Video_Depth, value);
	}

	//public bool UnitTestBareMetal
	//{
	//	get => Settings.GetValue(Name.UnitTest_BareMetal, false);
	//	set => Settings.SetValue(Name.UnitTest_BareMetal, value);
	//}

	#endregion Properties

	public MosaSettings()
	{
		Settings = new Settings();
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

	public void SetDetfaultSettings()
	{
		TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");

		MethodScanner = false;
		Multithreading = true;
		Platform = "x86";
		Multithreading = true;
		BaseAddress = 0x00400000;
		EmitBinary = true;
		TraceLevel = 0;

		ImageFile = "%DEFAULT%";

		DebugFile = null;
		AsmFile = null;
		MapFile = null;
		NasmFile = null;
		InlinedFile = null;

		MethodScanner = false;

		EmitBinary = true;
		SSA = true;
		BasicOptimizations = true;
		ValueNumbering = true;
		SparseConditionalConstantPropagation = true;
		Devirtualization = true;
		BitTracker = true;
		LoopInvariantCodeMotion = true;
		LongExpansion = true;
		TwoPassOptimization = true;
		PlatformOptimizations = true;
		InlineMethods = true;
		InlineExplicit = true;
		InlineAggressiveMaximum = 24;
		InlineMaximum = 12;
		OptimizationBasicWindow = 5;

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

		MaxErrors = Constant.MaxErrors;
		ConnectionTimeOut = Constant.ConnectionTimeOut;
		MaxAttempts = Constant.MaxAttempts;

		PlugKorlib = true;

		LauncherStart = false;
		LauncherExit = false;
		Launcher = false;

		MultibootVideo = false;
		MultibootVideoWidth = 640;
		MultibootVideoHeight = 480;
		MultibootVideoDepth = 32;

		EmitSumbols = false;
		EmitStaticRelocations = false;
		EmitShortSymbolNames = false;

		LinkerFormat = "elf32";
	}

	public void NormalizeSettings()
	{
		ImageFormat = ImageFormat == null ? string.Empty : ImageFormat.ToLowerInvariant().Trim();
		FileSystem = FileSystem == null ? string.Empty : FileSystem.ToLowerInvariant().Trim();
		EmulatorSerial = EmulatorSerial == null ? string.Empty : EmulatorSerial.ToLowerInvariant().Trim();
		Emulator = Emulator == null ? string.Empty : Emulator.ToLowerInvariant().Trim();
		Platform = Platform == null ? string.Empty : Platform.ToLowerInvariant().Trim();
		LinkerFormat = LinkerFormat == null ? string.Empty : LinkerFormat.ToLowerInvariant().Trim();
	}

	public void UpdateFileAndPathSettings()
	{
		if (string.IsNullOrWhiteSpace(TemporaryFolder) || TemporaryFolder != "%DEFAULT%")
		{
			TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");
		}

		if (string.IsNullOrWhiteSpace(ImageFolder) || ImageFolder != "%DEFAULT%")
		{
			ImageFolder = TemporaryFolder;
		}

		if (string.IsNullOrWhiteSpace(DefaultFolder) || DefaultFolder != "%DEFAULT%")
		{
			if (OutputFile != null && OutputFile != "%DEFAULT%")
			{
				DefaultFolder = Path.GetDirectoryName(Path.GetFullPath(OutputFile));
			}
			else
			{
				DefaultFolder = TemporaryFolder;
			}
		}

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

		var defaultFolder = DefaultFolder;

		if (OutputFile is null or "%DEFAULT%")
		{
			OutputFile = Path.Combine(defaultFolder, $"{baseFilename}.bin");
		}

		if (ImageFile == "%DEFAULT%")
		{
			ImageFile = Path.Combine(ImageFolder, $"{baseFilename}.{ImageFormat}");
		}

		if (MapFile == "%DEFAULT%")
		{
			MapFile = Path.Combine(defaultFolder, $"{baseFilename}-map.txt");
		}

		if (CompileTimeFile == "%DEFAULT%")
		{
			CompileTimeFile = Path.Combine(defaultFolder, $"{baseFilename}-time.txt");
		}

		if (DebugFile == "%DEFAULT%")
		{
			DebugFile = Path.Combine(defaultFolder, $"{baseFilename}.debug");
		}

		if (InlinedFile == "%DEFAULT%")
		{
			InlinedFile = Path.Combine(defaultFolder, $"{baseFilename}-inlined.txt");
		}

		if (PreLinkHashFile == "%DEFAULT%")
		{
			PreLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-prelink-hash.txt");
		}

		if (PostLinkHashFile == "%DEFAULT%")
		{
			PostLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-postlink-hash.txt");
		}

		if (AsmFile == "%DEFAULT%")
		{
			AsmFile = Path.Combine(defaultFolder, $"{baseFilename}.asm");
		}

		if (NasmFile == "%DEFAULT%")
		{
			NasmFile = Path.Combine(defaultFolder, $"{baseFilename}.nasm");
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
				Settings.AddPropertyListValueIfNew("SearchPaths", path);
			}
		}
	}

	#region Customer Helpers

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

	#endregion Customer Helpers
}
