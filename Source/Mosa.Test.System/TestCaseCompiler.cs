/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;
using System.Collections.Generic;
using x86 = Mosa.Platform.x86;

namespace Mosa.Test.System
{
	public delegate void CCtor();

	internal class TestCaseCompiler : BaseCompiler
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
		private TestCaseCompiler(BaseArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, ILinker linker, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, new CompilationScheduler(typeSystem, true), internalTrace, linker, compilerOptions)
		{
			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new PlugStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				new MetadataStage(),
				new LinkerFinalizationStage(),
			});

			architecture.ExtendCompilerPipeline(Pipeline);
		}

		/// <summary>
		/// Compiles the specified type system.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <returns></returns>
		public static TestLinker Compile(ITypeSystem typeSystem)
		{
			BaseArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			IInternalTrace internalTrace = new BasicInternalTrace();
			//(internalTrace.CompilerEventListener as BasicCompilerEventListener).DebugOutput = true;
			//(internalTrace.CompilerEventListener as BasicCompilerEventListener).ConsoleOutput = true;
			//(internalTrace.CompilerEventListener as BasicCompilerEventListener).Quiet = false;

			ConfigurableTraceFilter filter = new ConfigurableTraceFilter();
			filter.MethodMatch = MatchType.None;
			filter.StageMatch = MatchType.None;
			filter.TypeMatch = MatchType.None;
			filter.ExcludeInternalMethods = false;

			internalTrace.TraceFilter = filter;

			var linker = new TestLinker();

			CompilerOptions compilerOptions = new CompilerOptions();

			TestCaseCompiler compiler = new TestCaseCompiler(architecture, typeSystem, typeLayout, internalTrace, linker, compilerOptions);
			compiler.Compile();

			return linker;
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <returns>
		/// An instance of a BaseMethodCompiler for the given type/method pair.
		/// </returns>
		public override BaseMethodCompiler CreateMethodCompiler(RuntimeMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet)
		{
			return new TestCaseMethodCompiler(this, method, basicBlocks, instructionSet);
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
				//cctor();
			}
		}

		/// <summary>
		/// Queues the ctor for invocation after compilation.
		/// </summary>
		/// <param name="cctor">The cctor.</param>
		public void QueueCCtorForInvocationAfterCompilation(CCtor cctor)
		{
			cctorQueue.Enqueue(cctor);
		}
	}
}