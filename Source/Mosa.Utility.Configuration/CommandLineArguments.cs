﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Configuration;

public static class CommandLineArguments
{
	public static List<Argument> Map { get; } = GetMap();

	private static List<Argument> GetMap()
	{
		var map = new List<Argument>
		{
			new Argument { Name = null, Setting =  Name.Compiler_SourceFiles, Value = null, IsList = true},

			new Argument { Name = "-threading", Setting = Name.Compiler_Multithreading, Value= "true"},
			new Argument { Name = "-threading-off", Setting = Name.Compiler_Multithreading, Value= "false"},
			new Argument { Name = "-threads", Setting = Name.Compiler_Multithreading_MaxThreads },
			new Argument { Name = "-settings", Setting ="Settings", Value= null, IsList = true},

			new Argument { Name = "-o", Setting = Name.Compiler_OutputFile},

			new Argument { Name = "-base", Setting = Name.Compiler_BaseAddress},
			new Argument { Name = "-scanner", Setting = Name.Compiler_MethodScanner, Value= "true"},
			new Argument { Name = "-no-code", Setting = Name.Compiler_Binary, Value= "false"},
			new Argument { Name = "-path", Setting = Name.SearchPaths, IsList = true},

			new Argument { Name = "-inline", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-inline-off", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-ssa", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-ssa-off", Setting = Name.Optimizations_SSA, Value= "false"},
			new Argument { Name = "-sccp", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-sccp-off", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-basic-optimizations", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-basic-optimizations-off", Setting = Name.Optimizations_Basic, Value= "false"},
			new Argument { Name = "-inline-explicit", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-inline-explicit-off", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-long-expansion", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-long-expansion-off", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-two-pass", Setting = Name.Optimizations_TwoPass, Value= "true"},
			new Argument { Name = "-two-pass-off", Setting = Name.Optimizations_TwoPass, Value= "true"},
			new Argument { Name = "-value-numbering", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-value-numbering-off", Setting = Name.Optimizations_ValueNumbering, Value= "false"},
			new Argument { Name = "-loop-invariant-code-motion", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-loop-invariant-code-motion-off", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-platform-optimizations", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-platform-optimizations-off", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-bit-tracker", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-bit-tracker-off", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-bittracker", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-bittracker-off", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-devirtualization", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-devirtualization-off", Setting = Name.Optimizations_Devirtualization, Value= "false"},
			new Argument { Name = "-inline-level", Setting = Name.Optimizations_Inline_Maximum},
			new Argument { Name = "-basic-optimization-window", Setting = Name.Optimizations_Basic_Window},

			// Compiler - Platforms:
			new Argument { Name = "-platform", Setting = Name.Compiler_Platform},
			new Argument { Name = "-x86", Setting = Name.Compiler_Platform, Value= "x86"},
			new Argument { Name = "-x64", Setting = Name.Compiler_Platform, Value= "x64"},
			new Argument { Name = "-arm32", Setting = Name.Compiler_Platform, Value= "ARM32"},
			new Argument { Name = "-arm64", Setting = Name.Compiler_Platform, Value= "ARM64"},

			// Compiler - Debug Output:
			new Argument { Name = "-output-nasm", Setting = Name.CompilerDebug_NasmFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-asm", Setting = Name.CompilerDebug_AsmFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-map", Setting = Name.CompilerDebug_MapFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-counters", Setting = Name.CompilerDebug_CounterFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-time", Setting = Name.CompilerDebug_CompileTimeFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-debug", Setting = Name.CompilerDebug_DebugFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-inlined", Setting = Name.CompilerDebug_InlinedFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-hash", Setting = Name.CompilerDebug_PreLinkHashFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-hash", Setting = Name.CompilerDebug_PostLinkHashFile, Value= "%DEFAULT%"},
			new Argument { Name = "-output-debug-file", Setting = Name.CompilerDebug_DebugFile},
			new Argument { Name = "-asm", Setting = Name.CompilerDebug_AsmFile, Value= "%DEFAULT%"},
			new Argument { Name = "-map", Setting = Name.CompilerDebug_MapFile, Value= "%DEFAULT%"},
			new Argument { Name = "-counters-filter", Setting = Name.CompilerDebug_CounterFilter},

			// Compiler - Debug:
			new Argument { Name = "-inline-exclude", Setting = Name.Optimizations_Inline_Exclude, IsList = true},
			new Argument { Name = "-test-filter", Setting = Name.CompilerDebug_TestFilter},
			new Argument { Name = "-check", Setting = Name.CompilerDebug_FullCheckMode, Value= "true"},

			new Argument { Name = "-interrupt-method", Setting = Name.X86_InterruptMethodName},

			// Linker:
			new Argument { Name = "-emit-all-symbols", Setting = Name.Linker_Symbols, Value= "true"},
			new Argument { Name = "-emit-all-symbols-off", Setting = Name.Linker_Symbols, Value= "false"},
			new Argument { Name = "-emit-relocations", Setting = Name.Linker_StaticRelocations, Value= "true"},
			new Argument { Name = "-emit-relocations-off", Setting = Name.Linker_StaticRelocations, Value= "false"},
			new Argument { Name = "-emit-static-relocations", Setting = Name.Linker_StaticRelocations, Value= "true"},
			new Argument { Name = "-emit-dwarf", Setting = Name.Linker_Dwarf, Value= "true"},
			new Argument { Name = "-emit-dwarf-off", Setting = Name.Linker_Dwarf, Value= "false"},
			new Argument { Name = "-dwarf", Setting = Name.Linker_Dwarf, Value= "true"},

			// Explorer:
			new Argument { Name = "-filter", Setting = Name.Explorer_Filter, Value= null},
			new Argument { Name = "-explorer-debug", Setting = Name.Explorer_DebugDiagnostic, Value= "true"},
			new Argument { Name = "-autostart", Setting = Name.Explorer_Start, Value= "true"},

			// Launcher:
			new Argument { Name = "-autoexit", Setting = Name.Launcher_Exit, Value= "true"},
			new Argument { Name = "-autoexit-off", Setting = Name.Launcher_Exit, Value= "false"},
			new Argument { Name = "-autostart", Setting = Name.Launcher_Start, Value= "true"},
			new Argument { Name = "-autostart-off", Setting = Name.Launcher_Start, Value= "false"},
			new Argument { Name = "-autolaunch", Setting = Name.Launcher_Launch, Value="true"},
			new Argument { Name = "-autolaunch-off", Setting = Name.Launcher_Launch, Value="false"},

			new Argument { Name = "-destination", Setting = Name.Image_Folder},
			new Argument { Name = "-dest", Setting = Name.Image_Folder},

			new Argument { Name = "-launch", Setting = Name.Launcher_Launch, Value="true"},
			new Argument { Name = "-launch-off", Setting = Name.Launcher_Launch, Value="false"},

			// Launcher - Emulator:
			new Argument { Name = "-emulator", Setting = Name.Emulator},
			new Argument { Name = "-qemu", Setting = Name.Emulator, Value="qemu"},
			new Argument { Name = "-vmware", Setting = Name.Emulator, Value="vmware"},
			new Argument { Name = "-bochs", Setting = Name.Emulator, Value="bochs"},
			new Argument { Name = "-virtualbox", Setting = Name.Emulator, Value="virtualbox"},
			new Argument { Name = "-display", Setting = Name.Emulator_Display, Value= "on"},
			new Argument { Name = "-display-off", Setting = Name.Emulator_Display, Value= "off"},
			new Argument { Name = "-memory", Setting = Name.Emulator_Memory},
			new Argument { Name = "-cores", Setting = Name.Emulator_Cores},
			new Argument { Name = "-gdb", Setting = Name.Emulator_GDB, Value="true"},

			// Launcher - Image:
			new Argument { Name = "-vhd", Setting = Name.Image_Format, Value="vhd"},
			new Argument { Name = "-img", Setting = Name.Image_Format, Value="img"},
			new Argument { Name = "-vdi", Setting = Name.Image_Format, Value="vdi"},
			new Argument { Name = "-vmdk", Setting = Name.Image_Format, Value="vmdk"},
			new Argument { Name = "-image", Setting = Name.Image_ImageFile},

			new Argument { Name = "-blocks", Setting = Name.Image_DiskBlocks},
			new Argument { Name = "-volume-label", Setting = Name.Image_VolumeLabel},
			new Argument { Name = "-mbr", Setting = Name.Image_MasterBootRecordFile},
			new Argument { Name = "-boot", Setting = Name.Image_BootBlockFile},

			new Argument { Name = "-m", Setting = Name.Image_MasterBootRecordFile},
			new Argument { Name = "-b", Setting = Name.Image_BootBlockFile},

			// Launcher - Boot:
			new Argument { Name = "-multiboot-v2", Setting = Name.Multiboot_Version, Value= "v2"},
			new Argument { Name = "-multiboot-none", Setting = Name.Multiboot_Version, Value= ""},
			new Argument { Name = "-multiboot", Setting = Name.Multiboot_Version, Value= null},

			// Launcher - Serial:
			new Argument { Name = "-serial-connection", Setting = Name.Emulator_Serial},
			new Argument { Name = "-serial-pipe", Setting = Name.Emulator_Serial, Value= "pipe"},
			new Argument { Name = "-serial-tcpclient", Setting = Name.Emulator_Serial, Value= "tcpclient"},
			new Argument { Name = "-serial-tcpserver", Setting = Name.Emulator_Serial, Value= "tcpserver"},
			new Argument { Name = "-serial-connection-port", Setting = Name.Emulator_Serial_Port},
			new Argument { Name = "-serial-connection-host", Setting = Name.Emulator_Serial_Host},

			new Argument { Name = "-video", Setting = Name.Multiboot_Video, Value= "true"},
			new Argument { Name = "-video-width", Setting = Name.Multiboot_Video_Width},
			new Argument { Name = "-video-height", Setting = Name.Multiboot_Video_Height},

			new Argument { Name = "-vmware-svga", Setting = Name.Emulator_SVGA, Value="vmware"},
			new Argument { Name = "-virtio-vga", Setting = Name.Emulator_SVGA, Value="virtio"},

			new Argument { Name = "-gdb-port", Setting = Name.GDB_Port},
			new Argument { Name = "-gdb-host", Setting = Name.GDB_Host},

			new Argument { Name = "-osname", Setting = Name.OS_Name},
			new Argument { Name = "-bootoptions", Setting = Name.OS_BootOptions},
			new Argument { Name = "-bootloader-timeout", Setting = Name.BootLoaderTimeout},

			new Argument { Name = "-launch-gdb", Setting = Name.Launcher_GDB, Value="true"},
			new Argument { Name = "-launch-debugger", Setting = Name.Launcher_Debugger, Value="true"},

			new Argument { Name = "-output-serial-connection", Setting = Name.Launcher_Serial},
			new Argument { Name = "-output-serial-file", Setting = Name.Launcher_Serial_File},
			new Argument { Name = "-debug", Setting = Name.Launcher_Serial, Value="true"},
			new Argument { Name = "-debug", Setting = Name.OS_BootOptions, Value="bootoptions=serialdebug"},

			new Argument { Name = "-timeout", Setting = Name.Emulator_MaxRuntime},

			// Base directory is the output directory
			new Argument { Name = "-include", Setting = Name.Image_FileSystem_RootInclude },

			new Argument { Name = "-bios", Setting = Name.Image_Firmware, Value="bios"},
			new Argument { Name = "-uefi", Setting = Name.Image_Firmware, Value="uefi"},
			new Argument { Name = "-firmware", Setting = Name.Image_Firmware},

			// Advanced:
			new Argument { Name = "-plug-kernel", Setting = Name.Launcher_PlugKernel, Value= "true"},
			new Argument { Name = "-plug-korlib", Setting = Name.Launcher_PlugKorlib, Value= "true"},
			new Argument { Name = "-test", Setting = Name.Launcher_Test, Value= "true"},
			new Argument { Name = "-test", Setting = Name.OS_BootOptions, Value="bootoptions=serialdebug"},

			// Debugger:
			new Argument { Name = "-breakpoints", Setting = Name.Debugger_BreakpointFile},
			new Argument { Name = "-watch", Setting = Name.Debugger_WatchFile},

			// Unit Test:
			new Argument { Name = "-filter", Setting = Name.UnitTest_Filter, Value= null},
			new Argument { Name = "-maxerrors", Setting = Name.UnitTest_MaxErrors},

			// Optimization Levels:
			new Argument { Name = "-o0", Setting = Name.Optimizations_Basic, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_SSA, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_ValueNumbering, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Devirtualization, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o0", Setting = Name.Optimizations_Basic_Window, Value= "1"},

			new Argument { Name = "-o1", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_SSA, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_ValueNumbering, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o1", Setting = Name.Optimizations_Basic_Window, Value= "1"},

			new Argument { Name = "-o2", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o2", Setting = Name.Optimizations_Basic_Window, Value= "1"},

			new Argument { Name = "-o3", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o3", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o4", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o4", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o5", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-o5", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o6", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Inline_Maximum, Value= "5"},
			new Argument { Name = "-o6", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o7", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Inline_Maximum, Value= "10"},
			new Argument { Name = "-o7", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o8", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Inline_Maximum, Value= "10"},
			new Argument { Name = "-o8", Setting = Name.Optimizations_Basic_Window, Value= "5"},

			new Argument { Name = "-o9", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_TwoPass, Value= "true"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Inline_Maximum, Value= "12"},
			new Argument { Name = "-o9", Setting = Name.Optimizations_Basic_Window, Value= "10"},

			new Argument { Name = "-oNone", Setting = Name.Optimizations_Basic, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_SSA, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_ValueNumbering, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Devirtualization, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-oNone", Setting = Name.Optimizations_Basic_Window, Value= "1"},

			new Argument { Name = "-oMax", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_TwoPass, Value= "true"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Inline_Maximum, Value= "12"},
			new Argument { Name = "-oMax", Setting = Name.Optimizations_Basic_Window, Value= "20"},

			new Argument { Name = "-oSize", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_SCCP, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_LongExpansion, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Platform, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Inline, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Inline_Explicit, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_BitTracker, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_TwoPass, Value= "true"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Inline_Maximum, Value= "3"},
			new Argument { Name = "-oSize", Setting = Name.Optimizations_Basic_Window, Value= "10"},

			new Argument { Name = "-oFast", Setting = Name.Optimizations_Basic, Value= "true"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_SSA, Value= "true"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_ValueNumbering, Value= "true"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_SCCP, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Devirtualization, Value= "true"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_LongExpansion, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Platform, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Inline, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Inline_Explicit, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_LoopInvariantCodeMotion, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_BitTracker, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_TwoPass, Value= "false"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Inline_Maximum, Value= "0"},
			new Argument { Name = "-oFast", Setting = Name.Optimizations_Basic_Window, Value= "1"},
		};

		return map;
	}
}
