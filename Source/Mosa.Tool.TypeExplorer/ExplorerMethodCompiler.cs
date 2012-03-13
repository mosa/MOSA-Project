/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tool.TypeExplorer
{
	class ExplorerMethodCompiler : BaseMethodCompiler
	{
		private IntPtr address = IntPtr.Zero;

		public ExplorerMethodCompiler(ExplorerAssemblyCompiler assemblyCompiler, IArchitecture architecture, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, IInternalTrace internalTrace)
			: base(type, method, assemblyCompiler.Pipeline.FindFirst<IAssemblyLinker>(), architecture, assemblyCompiler.TypeSystem, assemblyCompiler.TypeLayout, null, compilationScheduler, internalTrace)
		{

			// Populate the pipeline
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandDeterminationStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				
				//new DominanceCalculationStage(),
				//new PhiPlacementStage(),
				//new EnterSSA(),
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PreFolding),
				//new ConstantFoldingStage(),
				//new ConstantPropagationStage(ConstantPropagationStage.PropagationStage.PostFolding),
				//new LeaveSSA(),

				new StackLayoutStage(),
				new PlatformStubStage(),

				new LoopAwareBlockOrderStage(),
				new CodeGenerationStage(),
			});
		}

	}
}
