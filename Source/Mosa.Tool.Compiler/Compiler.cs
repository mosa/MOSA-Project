// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;

using Mosa.Compiler.Framework.Linker.Elf.Dwarf;
using Mosa.Compiler.Framework.Trace;
using Mosa.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Tool.Compiler
{
	/// <summary>
	/// Class containing the Compiler.
	/// </summary>
	public class Compiler
	{
		#region Data

		protected MosaCompiler compiler;

		protected Settings Settings = new Settings();

		private DateTime CompileStartTime;

		private readonly string codeName = "Neptune";

		/// <summary>
		/// A string holding a simple usage description.
		/// </summary>
		private readonly string usageString;

		#endregion Data

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the Compiler class.
		/// </summary>
		public Compiler()
		{
			usageString = @"Usage: Mosa.Tool.Compiler.exe -o outputfile --platform [x86|x64] {additional options} inputfiles.

Example: Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin -platform x86 Mosa.HelloWorld.x86.exe mscorlib.dll Mosa.Plug.Korlib.dll Mosa.Plug.Korlib.x86.dll";
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Runs the command line parser and the compilation process.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		public void Run(string[] args)
		{
			RegisterPlatforms();

			// always print header with version information
			Console.WriteLine("MOSA Compiler, Version {0}.", CompilerVersion.VersionString);
			Console.WriteLine("Copyright 2020 by the MOSA Project. Licensed under the New BSD License.");

			Console.WriteLine();
			Console.WriteLine("Parsing options...");

			try
			{
				LoadArguments(args);

				var sourceFiles = Settings.GetValueList("Compiler.SourceFiles");

				if (sourceFiles == null && sourceFiles.Count == 0)
				{
					throw new Exception("No input file(s) specified.");
				}

				compiler = new MosaCompiler(Settings, CreateCompilerHooks());

				if (string.IsNullOrEmpty(compiler.CompilerSettings.OutputFile))
				{
					throw new Exception("No output file specified.");
				}

				if (compiler.CompilerSettings.Platform == null)
				{
					throw new Exception("No Architecture specified.");
				}

				Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
				Debug.AutoFlush = true;

				Console.WriteLine($" > Output file: {compiler.CompilerSettings.OutputFile}");
				Console.WriteLine($" > Input file(s): {(string.Join(", ", new List<string>(compiler.CompilerSettings.SourceFiles.ToArray())))}");
				Console.WriteLine($" > Platform: {compiler.CompilerSettings.Platform}");

				Console.WriteLine();
				Console.WriteLine("Compiling ...");
				Console.WriteLine();

				Compile();
			}
			catch (CompilerException ce)
			{
				ShowError(ce.Message);
				Environment.Exit(1);
				return;
			}
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		#endregion Public Methods

		#region Private Methods

		private void LoadArguments(string[] args)
		{
			SetDefaultSettings();

			var arguments = SettingsLoader.RecursiveReader(args);

			Settings.Merge(arguments);

			var sourcefiles = Settings.GetValueList("Compiler.SourceFiles");

			if (sourcefiles != null)
			{
				foreach (var sourcefile in sourcefiles)
				{
					var full = Path.GetFullPath(sourcefile);
					var path = Path.GetDirectoryName(full);

					if (!string.IsNullOrWhiteSpace(path))
					{
						Settings.AddPropertyListValue("SearchPaths", path);
					}
				}
			}

			SetDefault(Settings);
		}

		private void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Launcher.PlugKorlib", true);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("Optimizations.Basic", true);
			Settings.SetValue("Optimizations.BitTracker", true);
			Settings.SetValue("Optimizations.Inline", true);
			Settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			Settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			Settings.SetValue("Optimizations.Inline.Maximum", 12);
			Settings.SetValue("Optimizations.LongExpansion", true);
			Settings.SetValue("Optimizations.LoopInvariantCodeMotion", true);
			Settings.SetValue("Optimizations.Platform", true);
			Settings.SetValue("Optimizations.SCCP", true);
			Settings.SetValue("Optimizations.Devirtualization", true);
			Settings.SetValue("Optimizations.SSA", true);
			Settings.SetValue("Optimizations.TwoPass", true);
			Settings.SetValue("Optimizations.ValueNumbering", true);
			Settings.SetValue("Image.BootLoader", "syslinux3.72");
			Settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA"));
			Settings.SetValue("Image.Format", "IMG");
			Settings.SetValue("Image.FileSystem", "FAT16");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "TCPServer");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", 9999);
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Launcher.Start", false);
			Settings.SetValue("Launcher.Launch", false);
			Settings.SetValue("Launcher.Exit", false);
			Settings.SetValue("Launcher.HuntForCorLib", true);
		}

		private CompilerHooks CreateCompilerHooks()
		{
			CompileStartTime = DateTime.Now;

			var compilerHooks = new CompilerHooks
			{
				NotifyEvent = NotifyEvent,
			};

			return compilerHooks;
		}

		private void Compile()
		{
			compiler.Load();

			compiler.ThreadedCompile();
		}

		private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
		{
			if (compilerEvent != CompilerEvent.MethodCompileEnd
				&& compilerEvent != CompilerEvent.MethodCompileStart
				&& compilerEvent != CompilerEvent.Counter
				&& compilerEvent != CompilerEvent.SetupStageStart
				&& compilerEvent != CompilerEvent.SetupStageEnd
				&& compilerEvent != CompilerEvent.FinalizationStageStart
				&& compilerEvent != CompilerEvent.FinalizationStageEnd)
			{
				message = string.IsNullOrWhiteSpace(message) ? string.Empty : $": {message}";
				Console.WriteLine($"{(DateTime.Now - CompileStartTime).TotalSeconds:0.00} [{threadID.ToString()}] {compilerEvent.ToText()}{message}");
			}
		}

		/// <summary>
		/// Shows an error and a short information text.
		/// </summary>
		/// <param name="message">The error message to show.</param>
		private void ShowError(string message)
		{
			Console.WriteLine(usageString);
			Console.WriteLine();
			Console.Write("Error: ");
			Console.WriteLine(message);
			Console.WriteLine();
			Console.WriteLine("Execute 'Mosa.Tool.Compiler.exe --help' for more information.");
			Console.WriteLine();
		}

		private void SetDefault(Settings settings)
		{
			var compilerToolSettings = new CompilerToolSettings(settings);

			if (string.IsNullOrWhiteSpace(compilerToolSettings.TemporaryFolder) || compilerToolSettings.TemporaryFolder != "%DEFAULT%")
			{
				compilerToolSettings.TemporaryFolder = Path.Combine(Path.GetTempPath(), "MOSA");
			}

			if (string.IsNullOrWhiteSpace(compilerToolSettings.ImageFolder) || compilerToolSettings.ImageFolder != "%DEFAULT%")
			{
				compilerToolSettings.ImageFolder = compilerToolSettings.TemporaryFolder;
			}

			if (string.IsNullOrWhiteSpace(compilerToolSettings.DefaultFolder) || compilerToolSettings.DefaultFolder != "%DEFAULT%")
			{
				if (compilerToolSettings.OutputFile != null && compilerToolSettings.OutputFile != "%DEFAULT%")
				{
					compilerToolSettings.DefaultFolder = Path.GetDirectoryName(Path.GetFullPath(compilerToolSettings.OutputFile));
				}
				else
				{
					compilerToolSettings.DefaultFolder = compilerToolSettings.TemporaryFolder;
				}
			}

			var defaultFolder = compilerToolSettings.DefaultFolder;

			string baseFilename;

			if (compilerToolSettings.OutputFile != null && compilerToolSettings.OutputFile != "%DEFAULT%")
			{
				baseFilename = Path.GetFileNameWithoutExtension(compilerToolSettings.OutputFile);
			}
			else if (compilerToolSettings.SourceFiles != null && compilerToolSettings.SourceFiles.Count != 0)
			{
				baseFilename = Path.GetFileNameWithoutExtension(compilerToolSettings.SourceFiles[0]);
			}
			else
			{
				baseFilename = "_mosa_";
			}

			if (compilerToolSettings.OutputFile == null || compilerToolSettings.OutputFile == "%DEFAULT%")
			{
				compilerToolSettings.OutputFile = Path.Combine(defaultFolder, $"{baseFilename}.bin");
			}

			if (compilerToolSettings.ImageFile == "%DEFAULT%")
			{
				compilerToolSettings.ImageFile = Path.Combine(compilerToolSettings.ImageFolder, $"{baseFilename}.{compilerToolSettings.ImageFormat}");
			}

			if (compilerToolSettings.MapFile == "%DEFAULT%")
			{
				compilerToolSettings.MapFile = Path.Combine(defaultFolder, $"{baseFilename}-map.txt");
			}

			if (compilerToolSettings.CompileTimeFile == "%DEFAULT%")
			{
				compilerToolSettings.CompileTimeFile = Path.Combine(defaultFolder, $"{baseFilename}-time.txt");
			}

			if (compilerToolSettings.DebugFile == "%DEFAULT%")
			{
				compilerToolSettings.DebugFile = Path.Combine(defaultFolder, $"{baseFilename}.debug");
			}

			if (compilerToolSettings.InlinedFile == "%DEFAULT%")
			{
				compilerToolSettings.InlinedFile = Path.Combine(defaultFolder, $"{baseFilename}-inlined.txt");
			}

			if (compilerToolSettings.PreLinkHashFile == "%DEFAULT%")
			{
				compilerToolSettings.PreLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-prelink-hash.txt");
			}

			if (compilerToolSettings.PostLinkHashFile == "%DEFAULT%")
			{
				compilerToolSettings.PostLinkHashFile = Path.Combine(defaultFolder, $"{baseFilename}-postlink-hash.txt");
			}

			if (compilerToolSettings.AsmFile == "%DEFAULT%")
			{
				compilerToolSettings.AsmFile = Path.Combine(defaultFolder, $"{baseFilename}.asm");
			}

			if (compilerToolSettings.NasmFile == "%DEFAULT%")
			{
				compilerToolSettings.NasmFile = Path.Combine(defaultFolder, $"{baseFilename}.nasm");
			}
		}

		#endregion Private Methods
	}
}
