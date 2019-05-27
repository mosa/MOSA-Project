// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			const string platform = "x86";

			var compilerOptions = new CompilerOptions()
			{
				EnableSSA = true,
				EnableIROptimizations = true,
				EnableSparseConditionalConstantPropagation = true,
				EnableInlinedMethods = true,
				EnableIRLongExpansion = true,
				EnableValueNumbering = true,
				TwoPassOptimizations = true,
				EnableMethodScanner = true,
				EnableBitTracker = true, // FUTURE

				MultibootSpecification = MultibootSpecification.V1,
				LinkerFormatType = LinkerFormatType.Elf32,
				InlinedIRMaximum = 12,

				BaseAddress = 0x00500000,
				EmitStaticRelocations = false,
				EmitAllSymbols = false,

				EmitBinary = false,
				TraceLevel = 0,

				EnableStatistics = true,
			};

			compilerOptions.Architecture = SelectArchitecture(platform);

			compilerOptions.AddSourceFile($"Mosa.TestWorld.{platform}.exe");
			compilerOptions.AddSourceFile("Mosa.Plug.Korlib.dll");
			compilerOptions.AddSourceFile($"Mosa.Plug.Korlib.{platform}.dll");
			compilerOptions.TraceLevel = 5;

			var stopwatch = new Stopwatch();

			var compiler = new MosaCompiler(compilerOptions);

			compiler.Load();
			compiler.Initialize();
			compiler.Setup();

			stopwatch.Start();

			//MeasureCompileTime(stopwatch, compiler, "Mosa.Kernel.x86.IDT::SetTableEntries");
			//MeasureCompileTime(stopwatch, compiler, "System.Void Mosa.TestWorld.x86.Boot::Thread1");
			//MeasureCompileTime(stopwatch, compiler, "System.String System.Int32::CreateString(System.UInt32, System.Boolean, System.Boolean)");

			//compiler.ScheduleAll();

			var start = stopwatch.Elapsed.TotalSeconds;

			Console.WriteLine("Threaded Execution Time:");

			compiler.ThreadedCompile();

			//compiler.Execute();

			Console.WriteLine($"Elapsed: {(stopwatch.Elapsed.TotalSeconds - start).ToString("F2")} secs");

			Console.ReadKey();
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, string methodName)
		{
			var method = GetMethod(methodName, compiler.TypeSystem);

			MeasureCompileTime(stopwatch, compiler, method);
		}

		private static void MeasureCompileTime(Stopwatch stopwatch, MosaCompiler compiler, MosaMethod method)
		{
			Console.WriteLine($"Method: {method}");

			double min = double.MaxValue;

			for (int i = 0; i < 6; i++)
			{
				var start = stopwatch.Elapsed.TotalMilliseconds;

				compiler.CompileSingleMethod(method);

				var elapsed = stopwatch.Elapsed.TotalMilliseconds - start;

				min = Math.Min(min, elapsed);

				//Console.WriteLine($"Elapsed: {elapsed.ToString("F2")} ms");
			}

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

		private static BaseArchitecture SelectArchitecture(string architecture)
		{
			switch (architecture.ToLower())
			{
				case "x86": return Platform.x86.Architecture.CreateArchitecture(Platform.x86.ArchitectureFeatureFlags.AutoDetect);
				case "x64": return Platform.x64.Architecture.CreateArchitecture(Platform.x64.ArchitectureFeatureFlags.AutoDetect);
				case "armv6": return Platform.ARMv6.Architecture.CreateArchitecture(Platform.ARMv6.ArchitectureFeatureFlags.AutoDetect);
				default: throw new NotImplementCompilerException(string.Format("Unknown or unsupported Architecture {0}.", architecture));
			}
		}
	}
}
