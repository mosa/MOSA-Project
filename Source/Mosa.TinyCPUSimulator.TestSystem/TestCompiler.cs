// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class TestCompiler : ITraceListener
	{
		public MosaCompiler Compiler { get; private set; }

		public bool DebugOutput { get; set; }

		protected BaseTestPlatform platform;

		protected ISimAdapter simAdapter;

		protected SimLinker linker;

		protected const uint MaxTicks = 500000;
		internal bool IsInitialized { get; set; }

		internal TestCompiler(BaseTestPlatform platform)
		{
			this.platform = platform;
			DebugOutput = false;
			Reset();
		}

		public void Reset()
		{
			IsInitialized = false;

			simAdapter = platform.CreateSimAdaptor();

			Compiler = new MosaCompiler();

			Compiler.CompilerTrace.TraceFilter.Active = false;
			Compiler.CompilerTrace.TraceListener = this;

			Compiler.CompilerOptions.EnableOptimizations = true;
			Compiler.CompilerOptions.EnableSSA = true;
			Compiler.CompilerOptions.EnableVariablePromotion = true;
			Compiler.CompilerOptions.EnableSparseConditionalConstantPropagation = true;
			Compiler.CompilerOptions.EnableInlinedMethods = true;

			Compiler.CompilerOptions.Architecture = platform.CreateArchitecture();
			Compiler.CompilerOptions.LinkerFactory = delegate { return new SimLinker(simAdapter); };
			Compiler.CompilerFactory = delegate { return new SimCompiler(simAdapter); };
		}

		private void CompileTestCode()
		{
			platform.InitializeSimulation(simAdapter);

			var moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
			moduleLoader.LoadModuleFromFile("mscorlib.dll");
			moduleLoader.LoadModuleFromFile("Mosa.Platform.Internal." + platform.Name + ".dll");
			moduleLoader.LoadModuleFromFile("Mosa.Test.Collection.dll");
			moduleLoader.LoadModuleFromFile("Mosa.Kernel." + platform.Name + "Test.dll");

			Compiler.Load(TypeSystem.Load(moduleLoader.CreateMetadata()));

			//var threads = Compiler.CompilerOptions.UseMultipleThreadCompiler ? Environment.ProcessorCount : 1;
			//Compiler.Execute(threads);
			Compiler.Execute(Environment.ProcessorCount);

			linker = Compiler.Linker as SimLinker;
		}

		internal void Initialize()
		{
			if (IsInitialized)
				return;

			CompileTestCode();

			IsInitialized = true;  // must be before Run!

			simAdapter.SimCPU.Monitor.DebugOutput = false;

			Run<int>(string.Empty, "Default", Mosa.Compiler.Framework.Stages.TypeInitializerSchedulerStage.TypeInitializerName, true);

			simAdapter.SimCPU.Monitor.DebugOutput = DebugOutput;
		}

		public void DumpSymbols()
		{
			var symbols = simAdapter.SimCPU.Symbols;

			foreach (var symbol in symbols)
			{
				Debug.WriteLine(symbol.Value.ToString());
			}
		}

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			return Run<T>(ns, type, method, true, parameters);
		}

		protected T Run<T>(string ns, string type, string method, bool reset, params object[] parameters)
		{
			// enforce single thread execution only
			lock (this)
			{
				// If not initialized, do it now
				Initialize();

				// Find the test method to execute
				var runtimeMethod = FindMethod(
					ns,
					type,
					method,
					parameters
				);

				Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());

				var symbol = linker.GetSymbol(runtimeMethod.FullName, SectionKind.Text);

				ulong address = (ulong)symbol.VirtualAddress;

				//Console.Write("Testing: " + ns + "." + type + "." + method);

				if (reset)
				{
					// reset the stack
					platform.ResetSimulation(simAdapter);

					//Run<int>("Mosa.Kernel.x86Test", "KernelMemory", "SetMemory", false, new object[] { (uint)0x00900000 });
				}

				platform.PrepareToExecuteMethod(simAdapter, address, parameters);

				simAdapter.SimCPU.Monitor.BreakAtTick = simAdapter.SimCPU.Monitor.BreakAtTick + MaxTicks; // nothing should take this long
				simAdapter.SimCPU.Execute();

				if (simAdapter.SimCPU.Monitor.BreakAtTick == simAdapter.SimCPU.Tick)
				{
					throw new Exception("Aborted. Method did not complete under " + MaxTicks.ToString() + " ticks. " + simAdapter.SimCPU.Tick.ToString());
				}

				if (runtimeMethod.Signature.ReturnType.IsVoid)
					return default(T);

				object result = platform.GetResult(simAdapter, runtimeMethod.Signature.ReturnType);

				try
				{
					//Console.WriteLine(".");
					if (default(T) is ValueType)
						return (T)result;
					else
						return default(T);
				}
				catch (InvalidCastException e)
				{
					//Console.WriteLine("..." + e.ToString());
					Debug.Assert(false, String.Format("Failed to convert result {0} of destination {1} destination type {2}.", result, result.GetType(), typeof(T).ToString()));
					throw e;
				}
			}
		}

		private MosaMethod FindMethod(string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in Compiler.TypeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
						continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method)
					{
						if (m.Signature.Parameters.Count == parameters.Length)
						{
							return m;
						}
					}
				}

				break;
			}

			return null;
		}

		void ITraceListener.OnNewCompilerTraceEvent(CompilerEvent compilerStage, string info, int threadID)
		{
		}

		void ITraceListener.OnUpdatedCompilerProgress(int totalMethods, int completedMethods)
		{
		}

		void ITraceListener.OnNewTraceLog(TraceLog traceLog)
		{
		}
	}
}
