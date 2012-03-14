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
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;
using x86 = Mosa.Platform.x86;

namespace Mosa.Test.System
{
	public delegate void CCtor();

	class TestCaseAssemblyCompiler : AssemblyCompiler
	{
		private readonly Queue<CCtor> cctorQueue = new Queue<CCtor>();

		private readonly TestAssemblyLinker linker;

		private TestCaseAssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions) :
			base(architecture, typeSystem, typeLayout, internalTrace, compilerOptions)
		{
			linker = new TestAssemblyLinker();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new IAssemblyCompilerStage[] {
				new DelegateTypePatchStage(),
				new PlugStage(),
				new AssemblyMemberCompilationSchedulerStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				new MetadataStage(),
				linker
			});

			architecture.ExtendAssemblyCompilerPipeline(Pipeline);
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

			TestCaseAssemblyCompiler compiler = new TestCaseAssemblyCompiler(architecture, typeSystem, typeLayout, internalLog, compilerOptions);
			compiler.Compile();

			return compiler.linker;
		}

		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage schedulerStage, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new TestCaseMethodCompiler(this, schedulerStage, type, method);
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
