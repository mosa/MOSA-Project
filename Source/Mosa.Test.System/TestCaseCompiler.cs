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

		private readonly TestAssemblyLinker linker;

		/// <summary>
		/// Prevents a default instance of the <see cref="TestCaseCompiler"/> class from being created.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="typeLayout"></param>
		/// <param name="internalTrace"></param>
		/// <param name="compilerOptions"></param>
		private TestCaseCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, new MethodCompilerSchedulerStage(), internalTrace, compilerOptions)
		{
			linker = new TestAssemblyLinker();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new ICompilerStage[] {
				new DelegateTypePatchStage(),
				new PlugStage(),
				new TypeSchedulerStage(),
				(MethodCompilerSchedulerStage)base.Scheduler, // HACK
				new TypeLayoutStage(),
				new MetadataStage(),
				linker
			});

			architecture.ExtendCompilerPipeline(Pipeline);
		}

		public static TestAssemblyLinker Compile(ITypeSystem typeSystem)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			IInternalTrace internalLog = new BasicInternalTrace();
			(internalLog.CompilerEventListener as BasicCompilerEventListener).DebugOutput = false;
			(internalLog.CompilerEventListener as BasicCompilerEventListener).ConsoleOutput = false;

			CompilerOptions compilerOptions = new CompilerOptions();

			TestCaseCompiler compiler = new TestCaseCompiler(architecture, typeSystem, typeLayout, internalLog, compilerOptions);
			compiler.Compile();

			return compiler.linker;
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
			BaseMethodCompiler mc = new TestCaseMethodCompiler(this, method);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		protected override void EndCompile()
		{
			base.EndCompile();

			while (cctorQueue.Count > 0)
			{
				CCtor cctor = cctorQueue.Dequeue();
				cctor();
			}
		}

		public void QueueCCtorForInvocationAfterCompilation(CCtor cctor)
		{
			cctorQueue.Enqueue(cctor);
		}

	}
}
