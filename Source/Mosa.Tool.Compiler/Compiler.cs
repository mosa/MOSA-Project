// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Compiler.Platforms;
using Mosa.Utility.Configuration;

namespace Mosa.Tool.Compiler;

/// <summary>
/// Compiler
/// </summary>
public class Compiler
{
	#region Data

	private readonly Stopwatch Stopwatch = new();
	private readonly MosaSettings MosaSettings = new();

	#endregion Data

	#region Public Methods

	/// <summary>
	/// Runs the command line parser and the compilation process.
	/// </summary>
	/// <param name="args">The command line arguments.</param>
	public int Run(string[] args)
	{
		RegisterPlatforms();

		// always print header with version information
		Console.WriteLine("MOSA Compiler, Version {0}.", CompilerVersion.VersionString);
		Console.WriteLine("Copyright 2026 by the MOSA Project. Licensed under the New BSD License.");

		//OutputStatus($"Current Directory: {Environment.CurrentDirectory}");

		Stopwatch.Start();

		try
		{
			MosaSettings.LoadAppLocations();
			MosaSettings.SetDefaultSettings();
			MosaSettings.LoadArguments(args);
			MosaSettings.NormalizeSettings();
			MosaSettings.ResolveDefaults();
			SetRequiredSettings(MosaSettings);
			MosaSettings.ResolveFileAndPathSettings();
			MosaSettings.AddStandardPlugs();
			MosaSettings.ExpandSearchPaths();

			if (MosaSettings.SourceFiles == null || MosaSettings.SourceFiles.Count == 0)
			{
				OutputStatus("ERROR: No input file(s) specified.");
				return 1;
			}

			OutputStatus($"Compiling: {MosaSettings.SourceFiles[0]}");

			var compiler = new MosaCompiler(MosaSettings, CreateCompilerHooks(), new ClrModuleLoader(), new ClrTypeResolver());

			if (string.IsNullOrEmpty(compiler.MosaSettings.OutputFile))
			{
				OutputStatus("ERROR: No output file specified.");
				return 1;
			}

			if (compiler.MosaSettings.Platform == null)
			{
				OutputStatus("ERROR: No Architecture specified.");
				return 1;
			}

			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			Debug.AutoFlush = true;

			OutputStatus($"Input file(s): {string.Join(", ", new List<string>(compiler.MosaSettings.SourceFiles.ToArray()))}");
			//OutputStatus($"Search Folder(s): {string.Join(", ", new List<string>(compiler.MosaSettings.SearchPaths.ToArray()))}");
			OutputStatus($"Output file: {compiler.MosaSettings.OutputFile}");
			OutputStatus($"Available Cores: {Environment.ProcessorCount} | Max Threads: {compiler.MosaSettings.MaxThreads}");
			ReportProcessAffinity();
			OutputStatus($"Platform: {compiler.MosaSettings.Platform}");

			compiler.Load();

			compiler.Compile();
		}
		catch (Exception ex)
		{
			OutputStatus($"Exception: {ex.Message}");
			OutputStatus($"Exception: {ex.StackTrace}");
			return 1;
		}

		return 0;
	}

	private static void RegisterPlatforms()
	{
		PlatformRegistrations.Register();
	}

	#endregion Public Methods

	#region Private Methods

	private static void SetRequiredSettings(MosaSettings mosaSettings)
	{
		mosaSettings.LauncherExit = false;
	}

	private CompilerHooks CreateCompilerHooks()
	{
		var compilerHooks = new CompilerHooks
		{
			NotifyEvent = NotifyEvent,
		};

		return compilerHooks;
	}

	private void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (!CompilerHooks.IsStandardNotifyEvent(compilerEvent))
			return;

		if (compilerEvent == CompilerEvent.Diagnostic && !MosaSettings.Diagnostic)
			return;

		OutputStatus(CompilerHooks.GetStandardNotifyEventStatus(compilerEvent, message));
	}

	private void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private void ReportProcessAffinity()
	{
		OutputStatus($"Process ID: {Environment.ProcessId}");

		if (!OperatingSystem.IsWindows())
		{
			OutputStatus("Process affinity check is currently only reported on Windows.");
			return;
		}

		try
		{
			using var process = Process.GetCurrentProcess();

			var affinity = (ulong)process.ProcessorAffinity.ToInt64();
			var logicalCores = Environment.ProcessorCount;
			OutputStatus($"Process Affinity Mask: 0x{affinity:X}");

			if (logicalCores > 63)
			{
				OutputStatus("Affinity check is limited when more than 63 logical cores are available.");
				return;
			}

			var expectedMask = (1UL << logicalCores) - 1;
			OutputStatus($"Expected Full Mask: 0x{expectedMask:X}");

			if (affinity != expectedMask)
			{
				OutputStatus("WARNING: Process affinity appears restricted.");
			}
			else
			{
				OutputStatus("Process affinity appears unrestricted.");
			}
		}
		catch (Exception ex)
		{
			OutputStatus($"Process affinity check unavailable: {ex.Message}");
		}
	}

	#endregion Private Methods
}
