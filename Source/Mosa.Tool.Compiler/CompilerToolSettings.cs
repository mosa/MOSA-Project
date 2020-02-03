// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using System.Collections.Generic;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Compiler Options
	/// </summary>
	public class CompilerToolSettings
	{
		#region Properties

		public string AsmFile
		{
			get { return Settings.GetValue("CompilerDebug.AsmFile", null); }
			set { Settings.SetValue("CompilerDebug.AsmFile", value); }
		}

		public int BaseAddress
		{
			get { return Settings.GetValue("Compiler.BaseAddress", 0); }
			set { Settings.SetValue("Compiler.BaseAddress", value); }
		}

		public string Bochs
		{
			get { return Settings.GetValue("AppLocation.Bochs", null); }
			set { Settings.SetValue("AppLocation.Bochs", value); }
		}

		public string CompileTimeFile
		{
			get { return Settings.GetValue("CompilerDebug.CompileTimeFile", null); }
			set { Settings.SetValue("CompilerDebug.CompileTimeFile", value); }
		}

		public string DebugFile
		{
			get { return Settings.GetValue("CompilerDebug.DebugFile", null); }
			set { Settings.SetValue("CompilerDebug.DebugFile", value); }
		}

		public bool EmitBinary { get { return Settings.GetValue("Compiler.Binary", true); } }

		public bool EmitDrawf { get { return Settings.GetValue("Linker.Drawf", false); } }

		public string Emulator
		{
			get { return Settings.GetValue("Emulator", null); }
			set { Settings.SetValue("Emulator", value); }
		}

		public bool EmulatorDisplay
		{
			get { return Settings.GetValue("Emulator.Display", false); }
			set { Settings.SetValue("Emulator.Display", value); }
		}

		public bool EmulatorGDB
		{
			get { return Settings.GetValue("Emulator.GDB", false); }
			set { Settings.SetValue("Emulator.GDB", value); }
		}

		public int EmulatorMemory
		{
			get { return Settings.GetValue("Emulator.Memory", 128); }
			set { Settings.SetValue("Emulator.Memory", value); }
		}

		public string EmulatorSerial
		{
			get { return Settings.GetValue("Emulator.Serial", null); }
			set { Settings.SetValue("Emulator.Serial", value); }
		}

		public string EmulatorSerialHost
		{
			get { return Settings.GetValue("Emulator.Serial.Host", null); }
			set { Settings.SetValue("Emulator.Serial.Host", value); }
		}

		public string EmulatorSerialPipe
		{
			get { return Settings.GetValue("Emulator.Serial.Pipe", null); }
			set { Settings.SetValue("Emulator.Serial.Pipe", value); }
		}

		public int EmulatorSerialPort
		{
			get { return Settings.GetValue("Emulator.Serial.Port", 0); }
			set { Settings.SetValue("Emulator.Serial.Port", value); }
		}

		public string FileSystem
		{
			get { return Settings.GetValue("Image.FileSystem", null); }
			set { Settings.SetValue("Image.FileSystem", value); }
		}

		public string GDB
		{
			get { return Settings.GetValue("AppLocation.GDB", null); }
			set { Settings.SetValue("AppLocation.GDB", value); }
		}

		public string GDBHost
		{
			get { return Settings.GetValue("GDB.Host", "localhost"); }
			set { Settings.SetValue("GDB.Host", value); }
		}

		public int GDBPort
		{
			get { return Settings.GetValue("GDB.Port", 0); }
			set { Settings.SetValue("GDB.Port", value); }
		}

		public bool HuntForCorLib
		{
			get { return Settings.GetValue("Launcher.HuntForCorLib", false); }
			set { Settings.SetValue("Launcher.HuntForCorLib", value); }
		}

		public string ImageBootLoader
		{
			get { return Settings.GetValue("Image.BootLoader", null); }
			set { Settings.SetValue("Image.BootLoader", value); }
		}

		public string DefaultFolder
		{
			get { return Settings.GetValue("DefaultFolder", null); }
			set { Settings.SetValue("DefaultFolder", value); }
		}

		public string TemporaryFolder
		{
			get { return Settings.GetValue("TemporaryFolder", null); }
			set { Settings.SetValue("TemporaryFolder", value); }
		}

		public string ImageFolder
		{
			get { return Settings.GetValue("Image.Folder", null); }
			set { Settings.SetValue("Image.Folder", value); }
		}

		public string ImageFile
		{
			get { return Settings.GetValue("Image.ImageFile", null); }
			set { Settings.SetValue("Image.ImageFile", value); }
		}

		public string ImageFormat
		{
			get { return Settings.GetValue("Image.Format", null); }
			set { Settings.SetValue("Image.Format", value); }
		}

		public string InlinedFile
		{
			get { return Settings.GetValue("CompilerDebug.InlinedFile", null); }
			set { Settings.SetValue("CompilerDebug.InlinedFile", value); }
		}

		public bool LauncherExit
		{
			get { return Settings.GetValue("Launcher.Exit", false); }
			set { Settings.SetValue("Launcher.Exit", value); }
		}

		public bool LauncherStart
		{
			get { return Settings.GetValue("Launcher.Start", false); }
			set { Settings.SetValue("Launcher.Start", value); }
		}

		public bool LaunchGDB
		{
			get { return Settings.GetValue("Launcher.LaunchGDB", false); }
			set { Settings.SetValue("Launcher.LaunchGDB", value); }
		}

		public bool LaunchDebugger
		{
			get { return Settings.GetValue("Launcher.LaunchDebugger", false); }
			set { Settings.SetValue("Launcher.LaunchDebugger", value); }
		}

		public string LinkerFormat { get { return Settings.GetValue("Linker.Format", "elf32"); } }

		public string MapFile
		{
			get { return Settings.GetValue("CompilerDebug.MapFile", null); }
			set { Settings.SetValue("CompilerDebug.MapFile", value); }
		}

		public int MaxThreads { get { return Settings.GetValue("Compiler.Multithreading.MaxThreads", 0); } }

		public bool MethodScanner { get { return Settings.GetValue("Compiler.MethodScanner", false); } }

		public string Mkisofs
		{
			get { return Settings.GetValue("AppLocation.Mkisofs", null); }
			set { Settings.SetValue("AppLocation.Mkisofs", value); }
		}

		public bool Multithreading
		{
			get { return Settings.GetValue("Compiler.Multithreading", true); }
			set { Settings.SetValue("Compiler.Multithreading", value); }
		}

		public string NasmFile
		{
			get { return Settings.GetValue("CompilerDebug.NasmFile", null); }
			set { Settings.SetValue("CompilerDebug.NasmFile", value); }
		}

		public string Ndisasm

		{
			get { return Settings.GetValue("AppLocation.Ndisasm", null); }
			set { Settings.SetValue("AppLocation.Ndisasm", value); }
		}

		public string OutputFile
		{
			get { return Settings.GetValue("Compiler.OutputFile", null); }
			set { Settings.SetValue("Compiler.OutputFile", value); }
		}

		public string Platform
		{
			get { return Settings.GetValue("Compiler.Platform", "x86"); }
			set { Settings.SetValue("Compiler.Platform", value); }
		}

		public bool PlugKorlib
		{
			get { return Settings.GetValue("Launcher.PlugKorlib", false); }
			set { Settings.SetValue("Launcher.PlugKorlib", value); }
		}

		public string PostLinkHashFile
		{
			get { return Settings.GetValue("CompilerDebug.PostLinkHashFile", null); }
			set { Settings.SetValue("CompilerDebug.PostLinkHashFile", value); }
		}

		public string PreLinkHashFile
		{
			get { return Settings.GetValue("CompilerDebug.PreLinkHashFile", null); }
			set { Settings.SetValue("CompilerDebug.PreLinkHashFile", value); }
		}

		public string QEMU
		{
			get { return Settings.GetValue("AppLocation.Qemu", null); }
			set { Settings.SetValue("AppLocation.Qemu", value); }
		}

		public string QEMUBios
		{
			get { return Settings.GetValue("AppLocation.QemuBIOS", null); }
			set { Settings.SetValue("AppLocation.QemuBIOS", value); }
		}

		public string QemuImg
		{
			get { return Settings.GetValue("AppLocation.QemuImg", null); }
			set { Settings.SetValue("AppLocation.QemuImg", value); }
		}

		public List<string> SearchPaths { get { return Settings.GetValueList("SearchPaths"); } }

		public Settings Settings { get; } = new Settings();

		public List<string> SourceFiles { get { return Settings.GetValueList("Compiler.SourceFiles"); } }

		public string VmwarePlayer
		{
			get { return Settings.GetValue("AppLocation.VmwarePlayer", null); }
			set { Settings.SetValue("AppLocation.VmwarePlayer", value); }
		}

		#endregion Properties

		public CompilerToolSettings(Settings settings)
		{
			Settings.Merge(settings);
		}
	}
}
