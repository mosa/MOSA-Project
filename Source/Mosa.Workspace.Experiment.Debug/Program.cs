// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Common.Configuration;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Utility.Configuration;

namespace Mosa.Workspace.Experiment.Debug;

internal static class Program
{
	private static void Main()
	{
		Compile();
	}

	private static void Compile()
	{
		RegisterPlatforms();

		var mosaSettings = new MosaSettings
		{
			BaseAddress = 0x00500000,
			EmulatorSerialPort = 11111,
			LauncherStart = false,
			Launcher = false,
			LauncherExit = false,
			ImageFolder = Path.Combine(Path.GetTempPath(), "MOSA-UnitTest")
		};

		mosaSettings.AddSearchPath(AppContext.BaseDirectory);
		mosaSettings.AddSourceFile(Path.Combine(AppContext.BaseDirectory, "Mosa.UnitTests.x86.dll"));
		mosaSettings.AddSourceFile(Path.Combine(AppContext.BaseDirectory, "Mosa.Plug.Korlib.dll"));
		mosaSettings.AddSourceFile(Path.Combine(AppContext.BaseDirectory, "Mosa.Plug.Korlib.x86.dll"));

		var stopwatch = new Stopwatch();

		var compiler = new MosaCompiler(mosaSettings, new CompilerHooks(), new ClrModuleLoader(), new ClrTypeResolver());

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

		Console.WriteLine($"Elapsed: {stopwatch.Elapsed.TotalSeconds - start:F2} secs");

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

		Console.WriteLine($"Elapsed: {max:F2} ms (worst)");
		Console.WriteLine($"Elapsed: {avg:F2} ms (average)");
		Console.WriteLine($"Elapsed: {min:F2} ms (best)");
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
