// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using Mosa.Compiler.ARM32.Instructions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.MosaTypeSystem.CLR;
using Mosa.Compiler.Platforms;
using Mosa.Utility.Configuration;

namespace Mosa.Workspace.Experiment.Debug;

internal static class Program
{
	private static MosaSettings MosaSettings = new();
	private static Stopwatch Stopwatch = new();

	private static void Main()
	{
		Compile();
	}

	private static void SetRequiredSettings()
	{
		MosaSettings.BaseAddress = 0x00500000;
		MosaSettings.EmitBinary = true;
		MosaSettings.PlugKernel = true;
		MosaSettings.PlugKorlib = true;
		MosaSettings.Emulator = "qemu";
		MosaSettings.EmulatorMemory = 128;
		MosaSettings.EmulatorCores = 1;
		MosaSettings.Multithreading = true;
		MosaSettings.MaxThreads = Environment.ProcessorCount;
		MosaSettings.MethodScanner = false;
		MosaSettings.Launcher = true;
		MosaSettings.LauncherStart = false;
		MosaSettings.LauncherExit = true;
		MosaSettings.TraceLevel = 0;
		MosaSettings.Diagnostic = true;

		MosaSettings.AddSourceFile($"Mosa.UnitTests.BareMetal.{MosaSettings.Platform}.dll");
		MosaSettings.AddSourceFile("Mosa.UnitTests.dll");

		MosaSettings.EmulatorSerialPort = 11111;
		MosaSettings.LauncherStart = false;
		MosaSettings.Launcher = false;
		MosaSettings.LauncherExit = false;
		MosaSettings.ImageFolder = Path.Combine(Path.GetTempPath(), "MOSA-UnitTest");
		MosaSettings.AddSearchPath(AppContext.BaseDirectory);
	}

	public static void UpdateSettings()
	{
		MosaSettings.LoadAppLocations();
		MosaSettings.SetDefaultSettings();
		MosaSettings.NormalizeSettings();
		MosaSettings.ResolveDefaults();
		SetRequiredSettings();
		MosaSettings.ResolveFileAndPathSettings();
		MosaSettings.AddStandardPlugs();
		MosaSettings.ExpandSearchPaths();
	}

	private static void Compile()
	{
		Stopwatch.Start();
		OutputStatus("Initializing...");

		RegisterPlatforms();
		UpdateSettings();

		var compilerHook = CreateCompilerHook();
		var compiler = new MosaCompiler(MosaSettings, compilerHook, new ClrModuleLoader(), new ClrTypeResolver());

		compiler.Load();
		compiler.Initialize();
		compiler.Setup();

		RunSingleMethodPerformanceTest(compiler);
		RunMultithreadedFuzzPerformanceTest(compiler);
	}

	private static void RunMultithreadedFuzzPerformanceTest(MosaCompiler compiler)
	{
		var start = Stopwatch.Elapsed.TotalSeconds;

		const int targetCount = 100;
		var methods = GetFuzzyMethods(compiler.TypeSystem, targetCount);

		OutputStatus($"Scheduling {methods.Count} fuzzy methods...");
		foreach (var method in methods)
		{
			compiler.Schedule(method);
		}

		OutputStatus($"Compiling scheduled methods using {MosaSettings.MaxThreads} threads...");
		compiler.Compile(skipFinalization: true);

		OutputStatus($"Total Elapsed: {Stopwatch.Elapsed.TotalSeconds - start:F2} secs");
	}

	private static void RunSingleMethodPerformanceTest(MosaCompiler compiler)
	{
		var start = Stopwatch.Elapsed.TotalSeconds;

		MeasureCompileTime(Stopwatch, compiler, "Mosa.UnitTests.Fuzzy.Fuzz0009::FuzzMethod900");
		MeasureCompileTime(Stopwatch, compiler, "Mosa.UnitTests.Fuzzy.Fuzz0009::FuzzMethod901");
		MeasureCompileTime(Stopwatch, compiler, "Mosa.UnitTests.Fuzzy.Fuzz0009::FuzzMethod902");
		MeasureCompileTime(Stopwatch, compiler, "Mosa.UnitTests.Fuzzy.Fuzz0009::FuzzMethod903");
		MeasureCompileTime(Stopwatch, compiler, "Mosa.UnitTests.Fuzzy.Fuzz0009::FuzzMethod904");

		OutputStatus($"Elapsed: {Stopwatch.Elapsed.TotalSeconds - start:F2} secs");
	}

	private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, string methodName, int iterations = 100)
	{
		var method = GetMethod(methodName, compiler.TypeSystem);

		MeasureCompileTime(stopwatch, compiler, method, iterations);
	}

	private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, MosaMethod method, int iterations)
	{
		OutputStatus($"Method: {method}");

		double min = double.MaxValue;
		double max = double.MinValue;
		double total = 0;

		for (int i = 0; i < iterations; i++)
		{
			var start = Stopwatch.Elapsed.TotalMilliseconds;

			compiler.CompileSingleMethod(method);

			var elapsed = Stopwatch.Elapsed.TotalMilliseconds - start;

			min = Math.Min(min, elapsed);
			max = Math.Max(max, elapsed);
			total += elapsed;

			//OutputStatus($"Elapsed: {elapsed.ToString("F2")} ms");
		}

		double avg = total / iterations;

		OutputStatus($"  Interations: {iterations}");
		OutputStatus($"  Elapsed: {max:F2} ms (worst)");
		OutputStatus($"  Elapsed: {avg:F2} ms (average)");
		OutputStatus($"  Elapsed: {min:F2} ms (best)");
	}

	private static CompilerHooks CreateCompilerHook()
	{
		var compilerHooks = new CompilerHooks
		{
			NotifyEvent = NotifyEvent,
			NotifyProgress = NotifyProgress,
			NotifyStatus = NotifyStatus,
		};

		return compilerHooks;
	}

	private static void NotifyEvent(CompilerEvent compilerEvent, string message, int threadID)
	{
		if (compilerEvent is CompilerEvent.MethodCompileEnd
			or CompilerEvent.MethodCompileStart
			or CompilerEvent.Counter
			or CompilerEvent.SetupStageStart
			or CompilerEvent.SetupStageEnd
			or CompilerEvent.FinalizationStageStart
			or CompilerEvent.FinalizationStageEnd)
			return;

		if (compilerEvent == CompilerEvent.Diagnostic && !MosaSettings.Diagnostic)
			return;

		var eventName = compilerEvent.ToText();
		message = string.IsNullOrWhiteSpace(message) ? eventName : $"{eventName}: {message}";

		OutputStatus(message);
	}

	private static void NotifyProgress(int totalMethods, int completedMethods)
	{
	}

	private static void NotifyStatus(string status)
	{
		OutputStatus(status);
	}

	private static void RegisterPlatforms()
	{
		PlatformRegistrations.Register();
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

	private static List<MosaMethod> GetFuzzyMethods(TypeSystem typeSystem, int limit)
	{
		var methods = new List<MosaMethod>(limit);

		foreach (var type in typeSystem.AllTypes)
		{
			if (!type.FullName.Contains("Mosa.UnitTests.Fuzzy."))
				continue;

			foreach (var method in type.Methods)
			{
				if (!method.FullName.Contains("::FuzzMethod"))
					continue;

				methods.Add(method);

				if (methods.Count >= limit)
					return methods;
			}
		}

		return methods;
	}

	private static void OutputStatus(string status)
	{
		Console.WriteLine($"{Stopwatch.Elapsed.TotalSeconds:00.00} | {status}");
	}
}
