// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Common.Configuration;

namespace Mosa.Utility.Launcher;

/// <summary>
/// Compiler Options
/// </summary>
public class LauncherSettings
{
	#region Properties

	public string AsmFile
	{
		get => Settings.GetValue("CompilerDebug.AsmFile", null);
		set => Settings.SetValue("CompilerDebug.AsmFile", value);
	}

	public int BaseAddress
	{
		get => Settings.GetValue("Compiler.BaseAddress", 0);
		set => Settings.SetValue("Compiler.BaseAddress", value);
	}

	public string Bochs
	{
		get => Settings.GetValue("AppLocation.Bochs", null);
		set => Settings.SetValue("AppLocation.Bochs", value);
	}

	public string CompileTimeFile
	{
		get => Settings.GetValue("CompilerDebug.CompileTimeFile", null);
		set => Settings.SetValue("CompilerDebug.CompileTimeFile", value);
	}

	public string DebugFile
	{
		get => Settings.GetValue("CompilerDebug.DebugFile", null);
		set => Settings.SetValue("CompilerDebug.DebugFile", value);
	}

	public bool EmitBinary => Settings.GetValue("Compiler.Binary", true);

	public bool EmitDwarf => Settings.GetValue("Linker.Dwarf", false);

	public string MultibootVersion
	{
		get => Settings.GetValue("Multiboot.Version", "v1");
		set => Settings.SetValue("Multiboot.Version", value);
	}

	public string Emulator
	{
		get => Settings.GetValue("Emulator", null);
		set => Settings.SetValue("Emulator", value);
	}

	public bool EmulatorDisplay
	{
		get => Settings.GetValue("Emulator.Display", false);
		set => Settings.SetValue("Emulator.Display", value);
	}

	public bool EmulatorGDB
	{
		get => Settings.GetValue("Emulator.GDB", false);
		set => Settings.SetValue("Emulator.GDB", value);
	}

	public int EmulatorMemory
	{
		get => Settings.GetValue("Emulator.Memory", 128);
		set => Settings.SetValue("Emulator.Memory", value);
	}

	public int EmulatorCores
	{
		get => Settings.GetValue("Emulator.Cores", 1);
		set => Settings.SetValue("Emulator.Cores", value);
	}

	public string EmulatorSerial
	{
		get => Settings.GetValue("Emulator.Serial", null);
		set => Settings.SetValue("Emulator.Serial", value);
	}

	public string EmulatorSVGA
	{
		get => Settings.GetValue("Emulator.SVGA", "std");
		set => Settings.SetValue("Emulator.SVGA", value);
	}

	public string EmulatorSerialHost
	{
		get => Settings.GetValue("Emulator.Serial.Host", null);
		set => Settings.SetValue("Emulator.Serial.Host", value);
	}

	public string EmulatorSerialPipe
	{
		get => Settings.GetValue("Emulator.Serial.Pipe", null);
		set => Settings.SetValue("Emulator.Serial.Pipe", value);
	}

	public int EmulatorSerialPort
	{
		get => Settings.GetValue("Emulator.Serial.Port", 0);
		set => Settings.SetValue("Emulator.Serial.Port", value);
	}

	public string FileSystem
	{
		get => Settings.GetValue("Image.FileSystem", null);
		set => Settings.SetValue("Image.FileSystem", value);
	}

	public string GDB
	{
		get => Settings.GetValue("AppLocation.GDB", null);
		set => Settings.SetValue("AppLocation.GDB", value);
	}

	public string GDBHost
	{
		get => Settings.GetValue("GDB.Host", "localhost");
		set => Settings.SetValue("GDB.Host", value);
	}

	public int GDBPort
	{
		get => Settings.GetValue("GDB.Port", 0);
		set => Settings.SetValue("GDB.Port", value);
	}

	public string ImageFirmware
	{
		get => Settings.GetValue("Image.Firmware", null);
		set => Settings.SetValue("Image.Firmware", value);
	}

	public string ImageFolder
	{
		get => Settings.GetValue("Image.Folder", null);
		set => Settings.SetValue("Image.Folder", value);
	}

	public string DefaultFolder
	{
		get => Settings.GetValue("DefaultFolder", null);
		set => Settings.SetValue("DefaultFolder", value);
	}

	public string TemporaryFolder
	{
		get => Settings.GetValue("TemporaryFolder", null);
		set => Settings.SetValue("TemporaryFolder", value);
	}

	public string ImageFile
	{
		get => Settings.GetValue("Image.ImageFile", null);
		set => Settings.SetValue("Image.ImageFile", value);
	}

	public string ImageFormat
	{
		get => Settings.GetValue("Image.Format", null);
		set => Settings.SetValue("Image.Format", value);
	}

	public string InlinedFile
	{
		get => Settings.GetValue("CompilerDebug.InlinedFile", null);
		set => Settings.SetValue("CompilerDebug.InlinedFile", value);
	}

	public bool LauncherExit
	{
		get => Settings.GetValue("Launcher.Exit", false);
		set => Settings.SetValue("Launcher.Exit", value);
	}

	public bool LauncherStart
	{
		get => Settings.GetValue("Launcher.Start", false);
		set => Settings.SetValue("Launcher.Start", value);
	}

	public bool LaunchGDB
	{
		get => Settings.GetValue("Launcher.GDB", false);
		set => Settings.SetValue("Launcher.GDB", value);
	}

	public bool LaunchDebugger
	{
		get => Settings.GetValue("Launcher.Debugger", false);
		set => Settings.SetValue("Launcher.Debugger", value);
	}

	public string LinkerFormat => Settings.GetValue("Linker.Format", "elf32");

	public string MapFile
	{
		get => Settings.GetValue("CompilerDebug.MapFile", null);
		set => Settings.SetValue("CompilerDebug.MapFile", value);
	}

	public int MaxThreads => Settings.GetValue("Compiler.Multithreading.MaxThreads", 0);

	public bool MethodScanner => Settings.GetValue("Compiler.MethodScanner", false);

	public string Mkisofs
	{
		get => Settings.GetValue("AppLocation.Mkisofs", null);
		set => Settings.SetValue("AppLocation.Mkisofs", value);
	}

	public bool Multithreading
	{
		get => Settings.GetValue("Compiler.Multithreading", true);
		set => Settings.SetValue("Compiler.Multithreading", value);
	}

	public string NasmFile
	{
		get => Settings.GetValue("CompilerDebug.NasmFile", null);
		set => Settings.SetValue("CompilerDebug.NasmFile", value);
	}

	public string Ndisasm

	{
		get => Settings.GetValue("AppLocation.Ndisasm", null);
		set => Settings.SetValue("AppLocation.Ndisasm", value);
	}

	public string OutputFile
	{
		get => Settings.GetValue("Compiler.OutputFile", null);
		set => Settings.SetValue("Compiler.OutputFile", value);
	}

	public string Platform
	{
		get => Settings.GetValue("Compiler.Platform", "x86");
		set => Settings.SetValue("Compiler.Platform", value);
	}

	public bool PlugKorlib
	{
		get => Settings.GetValue("Launcher.PlugKorlib", false);
		set => Settings.SetValue("Launcher.PlugKorlib", value);
	}

	public string PostLinkHashFile
	{
		get => Settings.GetValue("CompilerDebug.PostLinkHashFile", null);
		set => Settings.SetValue("CompilerDebug.PostLinkHashFile", value);
	}

	public string PreLinkHashFile
	{
		get => Settings.GetValue("CompilerDebug.PreLinkHashFile", null);
		set => Settings.SetValue("CompilerDebug.PreLinkHashFile", value);
	}

	public string QEMU
	{
		get => Settings.GetValue("AppLocation.Qemu", null);
		set => Settings.SetValue("AppLocation.Qemu", value);
	}

	public string QEMUBios
	{
		get => Settings.GetValue("AppLocation.QemuBIOS", null);
		set => Settings.SetValue("AppLocation.QemuBIOS", value);
	}

	public string QEMUEdk2X86
	{
		get => Settings.GetValue("AppLocation.QemuEDK2X86", null);
		set => Settings.SetValue("AppLocation.QemuEDK2X86", value);
	}

	public string QEMUEdk2X64
	{
		get => Settings.GetValue("AppLocation.QemuEDK2X64", null);
		set => Settings.SetValue("AppLocation.QemuEDK2X64", value);
	}

	public string QEMUEdk2ARM
	{
		get => Settings.GetValue("AppLocation.QemuEDK2ARM", null);
		set => Settings.SetValue("AppLocation.QemuEDK2ARM", value);
	}

	public string QemuImg
	{
		get => Settings.GetValue("AppLocation.QemuImg", null);
		set => Settings.SetValue("AppLocation.QemuImg", value);
	}

	public bool LauncherTest
	{
		get => Settings.GetValue("Launcher.Test", false);
		set => Settings.SetValue("Launcher.Test", value);
	}

	public List<string> SearchPaths => Settings.GetValueList("SearchPaths");

	public Settings Settings { get; } = new Settings();

	public List<string> SourceFiles => Settings.GetValueList("Compiler.SourceFiles");

	public string FileSystemRootInclude
	{
		get => Settings.GetValue("Image.FileSystem.RootInclude", null);
		set => Settings.SetValue("Image.FileSystem.RootInclude", value);
	}

	public string VmwarePlayer
	{
		get => Settings.GetValue("AppLocation.VmwarePlayer", null);
		set => Settings.SetValue("AppLocation.VmwarePlayer", value);
	}

	public string VmwareWorkstation
	{
		get => Settings.GetValue("AppLocation.VmwareWorkstation", null);
		set => Settings.SetValue("AppLocation.VmwareWorkstation", value);
	}

	public string VirtualBox
	{
		get => Settings.GetValue("AppLocation.VirtualBox", null);
		set => Settings.SetValue("AppLocation.VirtualBox", value);
	}

	public string OSName
	{
		get => Settings.GetValue("OS.Name", null);
		set => Settings.SetValue("OS.Name", value);
	}

	#endregion Properties

	public LauncherSettings(Settings settings)
	{
		Settings.Merge(settings);
	}
}
