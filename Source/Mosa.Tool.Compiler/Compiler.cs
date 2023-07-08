// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Utility.Configuration;

namespace Mosa.Tool.Compiler;

/// <summary>
/// Class containing the Compiler.
/// </summary>
public class Compiler
{
	#region Data

	private DateTime CompileStartTime;

	/// <summary>
	/// A string holding a simple usage description.
	/// </summary>
	private readonly string usageString = @"Usage: Mosa.Tool.Compiler.exe -o outputfile --platform [x86|x64] {additional options} inputfiles.

Example: Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin -platform x86 Mosa.HelloWorld.x86.dll System.Runtime.dll Mosa.Plug.Korlib.dll Mosa.Plug.Korlib.x86.dll";

	#endregion Data

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
		Console.WriteLine("Copyright 2023 by the MOSA Project. Licensed under the New BSD License.");

		Console.WriteLine();
		Console.WriteLine("Parsing options...");

		try
		{
			var mosaSettings = new MosaSettings();

			mosaSettings.LoadAppLocations();
			mosaSettings.SetDetfaultSettings();
			mosaSettings.LoadArguments(args);
			SetRequiredSettings(mosaSettings);
			mosaSettings.ExpandSearchPaths();
			mosaSettings.NormalizeSettings();
			mosaSettings.UpdateFileAndPathSettings();

			if (mosaSettings.SourceFiles == null && mosaSettings.SourceFiles.Count == 0)
			{
				throw new Exception("No input file(s) specified.");
			}

			var compiler = new MosaCompiler(mosaSettings, CreateCompilerHooks(), new ClrModuleLoader(), new ClrTypeResolver());

			if (string.IsNullOrEmpty(compiler.MosaSettings.OutputFile))
			{
				throw new Exception("No output file specified.");
			}

			if (compiler.MosaSettings.Platform == null)
			{
				throw new Exception("No Architecture specified.");
			}

			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			Debug.AutoFlush = true;

			Console.WriteLine($" > Output file: {compiler.MosaSettings.OutputFile}");
			Console.WriteLine($" > Input file(s): {string.Join(", ", new List<string>(compiler.MosaSettings.SourceFiles.ToArray()))}");
			Console.WriteLine($" > Platform: {compiler.MosaSettings.Platform}");

			Console.WriteLine();
			Console.WriteLine("Compiling ...");
			Console.WriteLine();

			compiler.Load();

			compiler.Compile();
		}
		catch (Exception ce)
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

	private static void SetRequiredSettings(MosaSettings mosaSettings)
	{
		mosaSettings.LauncherExit = false;
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
			Console.WriteLine($"{(DateTime.Now - CompileStartTime).TotalSeconds:0.00} [{threadID}] {compilerEvent.ToText()}{message}");
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

	#endregion Private Methods
}
