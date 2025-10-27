// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;

namespace Mosa.Tool.Launcher.Console;

internal static class Program
{
	private static Builder Builder;

	private static readonly Stopwatch Stopwatch = new();

	internal static int Main(string[] args)
	{
		RegisterPlatforms();

		System.Console.WriteLine("MOSA Launcher, Version {0}.", CompilerVersion.VersionString);
		System.Console.WriteLine("Copyright 2025 by the MOSA Project. Licensed under the New BSD License.");

		//OutputStatus($"Current Directory: {Environment.CurrentDirectory}");

		Stopwatch.Start();

		try
		{
			var mosaSettings = new MosaSettings();

			mosaSettings.LoadAppLocations();
			mosaSettings.SetDefaultSettings();
			SetDefaultSettings(mosaSettings);
			mosaSettings.LoadArguments(args);
			mosaSettings.NormalizeSettings();
			mosaSettings.ResolveDefaults();
			SetRequiredSettings(mosaSettings);
			mosaSettings.ResolveFileAndPathSettings();
			mosaSettings.AddStandardPlugs();
			mosaSettings.ExpandSearchPaths();

			var compilerHooks = CreateCompilerHooks();

			Builder = new Builder(mosaSettings, compilerHooks);
			Builder.Build();

			if (!Builder.IsSucccessful)
			{
				OutputStatus("Aborting! A build error has occurred.");
				return 1;
			}

			if (mosaSettings.Launcher)
			{
				var starter = new Starter(Builder.MosaSettings, compilerHooks, Builder.Linker);

				if (!starter.Launch())
				{
					OutputStatus("Aborting! A launch error has occurred.");
					return 1;
				}
			}
		}
		catch (Exception ce)
		{
			OutputStatus($"Exception: {ce.Message}");
			OutputStatus($"Exception: {ce.StackTrace}");
			return 1;
		}

		return 0;
	}

	private static void SetDefaultSettings(MosaSettings mosaSettings)
	{
		mosaSettings.Launcher = true;
	}

	private static void SetRequiredSettings(MosaSettings mosaSettings)
	{
		mosaSettings.LauncherExit = true;
		mosaSettings.LauncherStart = true;
		mosaSettings.EmulatorDisplay = true;
	}

	private static void OutputStatus(string status)
	{
		System.Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}

	private static CompilerHooks CreateCompilerHooks()
	{
		var compilerHooks = new CompilerHooks
		{
			NotifyStatus = OutputStatus
		};

		return compilerHooks;
	}

	private static void RegisterPlatforms()
	{
		PlatformRegistry.Add(new Compiler.x86.Architecture());
		PlatformRegistry.Add(new Compiler.x64.Architecture());
		PlatformRegistry.Add(new Compiler.ARM32.Architecture());
	}
}
