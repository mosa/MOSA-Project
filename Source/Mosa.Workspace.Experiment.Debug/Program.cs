// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;
using System.IO;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			Compile();
		}

		private static void Compile()
		{
			RegisterPlatforms();

			var settings = new Settings();

			settings.SetValue("Compiler.MethodScanner", false);
			settings.SetValue("Compiler.Multithreading", true);
			settings.SetValue("Optimizations.SSA", false);
			settings.SetValue("Optimizations.Basic", false);
			settings.SetValue("Optimizations.ValueNumbering", false);
			settings.SetValue("Optimizations.SCCP", false);
			settings.SetValue("Optimizations.Devirtualization", false);
			settings.SetValue("Optimizations.BitTracker", false);
			settings.SetValue("Optimizations.LoopInvariantCodeMotion", false);
			settings.SetValue("Optimizations.LongExpansion", false);
			settings.SetValue("Optimizations.TwoPass", false);
			settings.SetValue("Optimizations.Platform", false);
			settings.SetValue("Optimizations.Inline", false);
			settings.SetValue("Optimizations.Inline.ExplicitOnly", false);
			settings.SetValue("Optimizations.Inline.Maximum", 12);
			settings.SetValue("Optimizations.Inline.AggressiveMaximum", 24);
			settings.SetValue("Multiboot.Version", "v1");
			settings.SetValue("Compiler.Platform", "x86");
			settings.SetValue("Compiler.BaseAddress", 0x00500000);
			settings.SetValue("Compiler.Binary", true);
			settings.SetValue("Compiler.TraceLevel", 0);
			settings.SetValue("Launcher.PlugKorlib", true);
			settings.SetValue("Multiboot.Version", "v1");
			settings.SetValue("Emulator", "Qemu");
			settings.SetValue("Emulator.Memory", 128);
			settings.SetValue("Emulator.Serial", "TCPServer");
			settings.SetValue("Emulator.Serial.Host", "127.0.0.1");
			settings.SetValue("Emulator.Serial.Port", new Random().Next(11111, 22222));
			settings.SetValue("Emulator.Serial.Pipe", "MOSA");
			settings.SetValue("Launcher.Start", false);
			settings.SetValue("Launcher.Launch", false);
			settings.SetValue("Launcher.Exit", true);
			settings.SetValue("Launcher.HuntForCorLib", true);
			settings.SetValue("Image.BootLoader", "syslinux3.72");
			settings.SetValue("Image.Folder", Path.Combine(Path.GetTempPath(), "MOSA-UnitTest"));
			settings.SetValue("Image.Format", "IMG");
			settings.SetValue("Image.FileSystem", "FAT16");

			settings.AddPropertyListValue("SearchPaths", AppContext.BaseDirectory);
			settings.AddPropertyListValue("Compiler.SourceFiles", Path.Combine(AppContext.BaseDirectory, "Mosa.UnitTests.x86.exe"));
			settings.AddPropertyListValue("Compiler.SourceFiles", Path.Combine(AppContext.BaseDirectory, "Mosa.Plug.Korlib.dll"));
			settings.AddPropertyListValue("Compiler.SourceFiles", Path.Combine(AppContext.BaseDirectory, "Mosa.Plug.Korlib.x86.dll"));

			var stopwatch = new Stopwatch();

			var compiler = new MosaCompiler(settings, new CompilerHooks());

			compiler.Load();
			compiler.Initialize();
			compiler.Setup();

			stopwatch.Start();

			MeasureCompileTime(stopwatch, compiler, "Mosa.Kernel.x86.IDT::SetTableEntries");

			compiler.ScheduleAll();

			var start = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine("Threaded Execution Time:");

			//compiler.ThreadedCompile();

			//compiler.Execute();

			Console.WriteLine($"Elapsed: {(stopwatch.Elapsed.TotalSeconds - start).ToString("F2")} secs");

			Console.ReadKey();
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());
			PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, string methodName, int iterations = 10)
		{
			var method = GetMethod(methodName, compiler.TypeSystem);

			MeasureCompileTime(stopwatch, compiler, method, iterations);
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, MosaMethod method, int iterations)
		{
			Console.WriteLine($"Method: {method}");

			double min = double.MaxValue;
			double max = double.MinValue;
			double total = 0;

			for (int i = 0; i < iterations; i++)
			{
				var start = stopwatch.Elapsed.TotalMilliseconds;

				compiler.CompileSingleMethod(method);

				var elapsed = stopwatch.Elapsed.TotalMilliseconds - start;

				min = Math.Min(min, elapsed);
				max = Math.Max(max, elapsed);
				total += elapsed;

				//Console.WriteLine($"Elapsed: {elapsed.ToString("F2")} ms");
			}

			double avg = total / iterations;

			Console.WriteLine($"Elapsed: {max.ToString("F2")} ms (worst)");
			Console.WriteLine($"Elapsed: {avg.ToString("F2")} ms (average)");
			Console.WriteLine($"Elapsed: {min.ToString("F2")} ms (best)");
		}

		private static MosaMethod GetMethod(string partial, TypeSystem typeSystem)
		{
			foreach (var type in typeSystem.AllTypes)
			{
				foreach (var method in type.Methods)
				{
					if (method.FullName.Contains(partial))
						return method;
				}
			}

			return null;
		}
	}
}
