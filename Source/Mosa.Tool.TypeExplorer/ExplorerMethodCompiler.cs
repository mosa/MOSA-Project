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
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tool.TypeExplorer
{
	class ExplorerMethodCompiler : BaseMethodCompiler
	{
		private IntPtr address = IntPtr.Zero;

		public ExplorerMethodCompiler(ExplorerAssemblyCompiler assemblyCompiler, ICompilationSchedulerStage compilationScheduler, RuntimeType type, RuntimeMethod method, CompilerOptions compilerOptions)
			: base(assemblyCompiler, type, method, null, compilationScheduler)
		{

			// Populate the pipeline
			this.Pipeline.AddRange(new IMethodCompilerStage[] {
				new DecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandDeterminationStage(),
				//new SingleUseMarkerStage(),
				//new OperandUsageAnalyzerStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSSA) ? new SSAOptimizations() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				
				//new StrengthReductionStage(),
				new StackLayoutStage(),
				new PlatformStubStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new SimpleRegisterAllocatorStage(),
				new CodeGenerationStage(),
			});
		}

	}
}
