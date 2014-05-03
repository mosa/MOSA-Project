/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class TestCompiler
	{
		protected BaseTestPlatform platform;
		protected ConfigurableTraceFilter filter = new ConfigurableTraceFilter();
		protected IInternalTrace internalTrace = new BasicInternalTrace();
		protected ISimAdapter adapter;
		protected ISimAdapter simAdapter;
		protected BaseArchitecture architecture;
		protected BaseLinker linker;
		protected TypeSystem typeSystem;
		protected MosaTypeLayout typeLayout;
		protected SimCompiler simCompiler;

		protected CompilerOptions compilerOptions = new CompilerOptions();

		public bool EnableSSA { get; set; }

		public bool EnableSSAOptimizations { get; set; }

		public TestCompiler(BaseTestPlatform platform)
		{
			this.platform = platform;

			filter.MethodMatch = MatchType.None;
			internalTrace.TraceFilter = filter;
			EnableSSA = true;
			EnableSSAOptimizations = true;

			architecture = platform.CreateArchitecture();
			simAdapter = platform.CreateSimAdaptor();

			linker = new SimLinker(simAdapter);
		}

		protected void CompileTestCode()
		{
			if (simCompiler != null)
				return;

			MosaModuleLoader moduleLoader = new MosaModuleLoader();

			moduleLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
			moduleLoader.LoadModuleFromFile("mscorlib.dll");
			moduleLoader.LoadModuleFromFile("Mosa.Platform.Internal." + platform.Name + ".dll");
			moduleLoader.LoadModuleFromFile("Mosa.Test.Collection.dll");
			moduleLoader.LoadModuleFromFile("Mosa.Kernel." + platform.Name + "Test.dll");

			typeSystem = TypeSystem.Load(moduleLoader.CreateMetadata());

			typeLayout = new MosaTypeLayout(typeSystem, 4, 4);

			compilerOptions.EnableSSA = EnableSSA;
			compilerOptions.EnableSSAOptimizations = EnableSSAOptimizations;

			compilerOptions.BaseAddress = 0x00400000; // default location

			platform.InitializeSimulation(simAdapter);

			simCompiler = SimCompiler.Compile(typeSystem, typeLayout, internalTrace, EnableSSA, architecture, simAdapter, linker);

			//simAdapter.SimCPU.Monitor.DebugOutput = true; // DEBUG OPTION

			Run<int>(string.Empty, "Default", "AssemblyInit", true);

			//simAdapter.SimCPU.Monitor.DebugOutput = true; // DEBUG OPTION
		}

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			return Run<T>(ns, type, method, true, parameters);
		}

		protected T Run<T>(string ns, string type, string method, bool reset, params object[] parameters)
		{
			CompileTestCode();

			if (reset)
			{
				// reset the stack
				platform.ResetSimulation(simAdapter);

				//Run<int>("Mosa.Kernel.x86Test", "KernelMemory", "SetMemory", false, new object[] { (uint)0x00900000 });
			}

			// Find the test method to execute
			MosaMethod runtimeMethod = FindMethod(
				ns,
				type,
				method,
				parameters
			);

			Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());

			var symbol = linker.GetSymbol(runtimeMethod.FullName, SectionKind.Text);

			ulong address = (ulong)symbol.VirtualAddress;

			platform.PopulateStack(simAdapter, parameters);

			platform.PrepareToExecuteMethod(simAdapter, address);

			simAdapter.SimCPU.Monitor.BreakAtTick = simAdapter.SimCPU.Tick + 100000; // nothing should take this long
			simAdapter.SimCPU.Execute();

			if (simAdapter.SimCPU.Monitor.BreakAtTick == simAdapter.SimCPU.Tick)
				throw new Exception("Aborted. Method did not complete under 100000 ticks. " + simAdapter.SimCPU.Tick.ToString());

			if (runtimeMethod.Signature.ReturnType.IsVoid)
				return default(T);

			object result = platform.GetResult(simAdapter, runtimeMethod.Signature.ReturnType);

			try
			{
				if (default(T) is ValueType)
					return (T)result;
				else
					return default(T);
			}
			catch (InvalidCastException e)
			{
				Debug.Assert(false, String.Format(@"Failed to convert result {0} of type {1} to type {2}.", result, result.GetType(), typeof(T).ToString()));
				throw e;
			}
		}

		private MosaMethod FindMethod(string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
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
	}
}