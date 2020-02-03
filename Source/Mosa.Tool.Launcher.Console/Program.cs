// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Utility.BootImage;
using Mosa.Utility.Configuration;
using Mosa.Utility.Launcher;
using System;
using System.IO;

namespace Mosa.Tool.Launcher.Console
{
	internal static class Program
	{
		private static Settings Settings;

		private static Builder Builder;

		internal static int Main(string[] args)
		{
			Settings = AppLocationsSettings.GetAppLocations();

			NotifyStatus($"Current Directory: {Environment.CurrentDirectory}");

			RegisterPlatforms();

			LoadArguments(args);

			try
			{
				var compilerHooks = CreateCompilerHooks();

				Builder = new Builder(Settings, compilerHooks);

				Builder.Build();

				if (Settings.GetValue("Launcher.Launch", false))
				{
					var starter = new Starter(Builder.Settings, compilerHooks, Builder.Linker);

					starter.Launch();
				}
			}
			catch (Exception e)
			{
				NotifyStatus($"Exception: {e.ToString()}");
				return 1;
			}

			return 0;
		}

		private static void SetDefaultSettings()
		{
			Settings.SetValue("Compiler.BaseAddress", 0x00400000);
			Settings.SetValue("Compiler.Binary", true);
			Settings.SetValue("Compiler.MethodScanner", false);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("Compiler.Platform", "x86");
			Settings.SetValue("Compiler.TraceLevel", 0);
			Settings.SetValue("Compiler.Multithreading", true);
			Settings.SetValue("CompilerDebug.DebugFile", string.Empty);
			Settings.SetValue("CompilerDebug.AsmFile", string.Empty);
			Settings.SetValue("CompilerDebug.MapFile", string.Empty);
			Settings.SetValue("CompilerDebug.NasmFile", string.Empty);
			Settings.SetValue("CompilerDebug.InlineFile", string.Empty);
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
			Settings.SetValue("Image.ImageFile", "%DEFAULT%");
			Settings.SetValue("Multiboot.Version", "v1");
			Settings.SetValue("Multiboot.Video", false);
			Settings.SetValue("Multiboot.Video.Width", 640);
			Settings.SetValue("Multiboot.Video.Height", 480);
			Settings.SetValue("Multiboot.Video.Depth", 32);
			Settings.SetValue("Emulator", "Qemu");
			Settings.SetValue("Emulator.Memory", 128);
			Settings.SetValue("Emulator.Serial", "none");
			Settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			Settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			Settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			Settings.SetValue("Emulator.Display", true);
			Settings.SetValue("Launcher.Start", true);
			Settings.SetValue("Launcher.Launch", true);
			Settings.SetValue("Launcher.Exit", true);
			Settings.SetValue("Launcher.PlugKorlib", true);
			Settings.SetValue("Launcher.HuntForCorLib", true);
			Settings.SetValue("Linker.Drawf", false);
		}

		public static void LoadArguments(string[] args)
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
		}

		private static void NotifyStatus(string status)
		{
			System.Console.WriteLine(status);
		}

		private static CompilerHooks CreateCompilerHooks()
		{
			var compilerHooks = new CompilerHooks
			{
				NotifyStatus = NotifyStatus
			};

			return compilerHooks;
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}
	}
}
