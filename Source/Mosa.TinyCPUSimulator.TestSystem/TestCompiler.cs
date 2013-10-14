/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.TypeSystem;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Diagnostics;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public class TestCompiler
	{
		protected BasePlatform platform;
		protected ConfigurableTraceFilter filter = new ConfigurableTraceFilter();
		protected IInternalTrace internalTrace = new BasicInternalTrace();
		protected ISimAdapter adapter;
		protected ISimAdapter simAdapter;
		protected IArchitecture architecture;
		protected ILinker linker;
		protected ITypeSystem typeSystem;
		protected ITypeLayout typeLayout;
		protected SimCompiler simCompiler;

		protected CompilerOptions compilerOptions = new CompilerOptions();

		public bool EnableSSA { get; set; }

		public bool EnableSSAOptimizations { get; set; }

		public TestCompiler(BasePlatform platform)
		{
			this.platform = platform;

			filter.MethodMatch = MatchType.None;
			internalTrace.TraceFilter = filter;
			EnableSSA = true;
			EnableSSAOptimizations = true;

			architecture = platform.CreateArchitecture();
			simAdapter = platform.CreateSimAdaptor();

			linker = new SimLinker(simAdapter);
			typeSystem = new TypeSystem();
		}

		protected void CompileTestCode()
		{
			if (simCompiler != null)
				return;

			IAssemblyLoader assemblyLoader = new AssemblyLoader();

			assemblyLoader.AddPrivatePath(System.IO.Directory.GetCurrentDirectory());
			assemblyLoader.LoadModule("mscorlib.dll");
			assemblyLoader.LoadModule("Mosa.Platform." + platform.Name + ".Intrinsic.dll");
			assemblyLoader.LoadModule("Mosa.Test.Collection.dll");
			assemblyLoader.LoadModule("Mosa.Kernel.x86Test.dll");

			typeSystem.LoadModules(assemblyLoader.Modules);

			typeLayout = new TypeLayout(typeSystem, 4, 4);

			compilerOptions.EnableSSA = EnableSSA;
			compilerOptions.EnableSSAOptimizations = EnableSSAOptimizations;

			simCompiler = SimCompiler.Compile(typeSystem, typeLayout, internalTrace, EnableSSA, architecture, simAdapter, linker);

			platform.InitializeSimulation(simAdapter);

			simAdapter.Monitor.EnableStepping = false;
			simAdapter.Monitor.BreakAtTick = 4000; // nothing should take this long
			//simAdapter.Monitor.DebugOutput = true;
		}

		public T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			CompileTestCode();

			// reset the stack
			platform.ResetSimulation(simAdapter);

			// Find the test method to execute
			RuntimeMethod runtimeMethod = FindMethod(
				ns,
				type,
				method,
				parameters
			);

			Debug.Assert(runtimeMethod != null, runtimeMethod.ToString());

			LinkerSymbol symbol = linker.GetSymbol(runtimeMethod.FullName);
			//LinkerSection section = linker.GetSection(symbol.SectionKind);

			ulong address = (ulong)symbol.VirtualAddress;

			platform.PopulateStack(simAdapter, parameters);

			platform.PrepareToExecuteMethod(simAdapter, address);

			simAdapter.Execute();

			object result = platform.GetResult(simAdapter, runtimeMethod.ReturnType.Type);

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

		private RuntimeMethod FindMethod(string ns, string type, string method, params object[] parameters)
		{
			foreach (RuntimeType t in typeSystem.GetAllTypes())
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns))
					if (t.Namespace != ns)
						continue;

				foreach (RuntimeMethod m in t.Methods)
				{
					if (m.Name == method)
					{
						if (m.Parameters.Count == parameters.Length)
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