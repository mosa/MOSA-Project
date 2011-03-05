/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;

using x86 = Mosa.Platform.x86;

namespace Mosa.Test.Runtime.CompilerFramework
{
	public delegate void CCtor();

	class TestCaseAssemblyCompiler : AssemblyCompiler
	{
		private readonly Queue<CCtor> cctorQueue = new Queue<CCtor>();

		private readonly TestAssemblyLinker linker;

		private TestCaseAssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout) :
			base(architecture, typeSystem, typeLayout)
		{
			linker = new TestAssemblyLinker();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new IAssemblyCompilerStage[] {
				new AssemblyMemberCompilationSchedulerStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				linker
			});
			architecture.ExtendAssemblyCompilerPipeline(Pipeline);
		}

		public static TestAssemblyLinker Compile(ITypeSystem typeSystem)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			TestCaseAssemblyCompiler compiler = new TestCaseAssemblyCompiler(architecture, typeSystem, typeLayout);
			compiler.Compile();

			return compiler.linker;
		}

		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage schedulerStage, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new TestCaseMethodCompiler(this, Architecture, schedulerStage, type, method);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		protected override void EndCompile()
		{
			base.EndCompile();

			while (this.cctorQueue.Count > 0)
			{
				CCtor cctor = this.cctorQueue.Dequeue();
				cctor();
			}
		}

		public void QueueCCtorForInvocationAfterCompilation(CCtor cctor)
		{
			cctorQueue.Enqueue(cctor);
		}

	}
}
