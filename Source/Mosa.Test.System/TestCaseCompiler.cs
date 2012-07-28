/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;
using x86 = Mosa.Platform.x86;

namespace Mosa.Test.System
{
	public delegate void CCtor();

	class TestCaseCompiler : BaseCompiler
	{
		private readonly Queue<CCtor> cctorQueue = new Queue<CCtor>();

		/// <summary>
		/// Prevents a default instance of the <see cref="TestCaseCompiler"/> class from being created.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout">The type layout.</param>
		/// <param name="internalTrace">The internal trace.</param>
		/// <param name="compilerOptions">The compiler options.</param>
		private TestCaseCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), internalTrace, compilerOptions)
		{
			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				new MetadataStage(),
				(TestAssemblyLinker)Linker
			});

			architecture.ExtendCompilerPipeline(Pipeline);
		}

		/// <summary>
		/// Compiles the specified type system.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <returns></returns>
		public static TestAssemblyLinker Compile(ITypeSystem typeSystem)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			IInternalTrace internalLog = new BasicInternalTrace();
			(internalLog.CompilerEventListener as BasicCompilerEventListener).DebugOutput = false;
			(internalLog.CompilerEventListener as BasicCompilerEventListener).ConsoleOutput = false;

			var linker = new TestAssemblyLinker();

			CompilerOptions compilerOptions = new CompilerOptions();
			compilerOptions.Linker = linker;

			TestCaseCompiler compiler = new TestCaseCompiler(architecture, typeSystem, typeLayout, internalLog, compilerOptions);
			compiler.Compile();

			return linker;
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public override BaseMethodCompiler CreateMethodCompiler(RuntimeMethod method)
		{
			return new TestCaseMethodCompiler(this, method);
		}

		/// <summary>
		/// Called when compilation has completed.
		/// </summary>
		protected override void EndCompile()
		{
			base.EndCompile();

			while (cctorQueue.Count > 0)
			{
				CCtor cctor = cctorQueue.Dequeue();
				cctor();
			}
		}

		/// <summary>
		/// Queues the C ctor for invocation after compilation.
		/// </summary>
		/// <param name="cctor">The cctor.</param>
		public void QueueCCtorForInvocationAfterCompilation(CCtor cctor)
		{
			cctorQueue.Enqueue(cctor);
		}

	}
}
