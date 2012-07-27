/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Tool.TypeExplorer
{
	class ExplorerMethodCompiler : BaseMethodCompiler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerMethodCompiler"/> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		public ExplorerMethodCompiler(ExplorerCompiler compiler, RuntimeMethod method)
			: base(compiler, method, null)
		{
			var compilerOptions = Compiler.CompilerOptions;

			// Populate the pipeline
			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				//new SingleUseMarkerStage(),
				//new OperandUsageAnalyzerStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				
				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new LocalVariablePromotionStage() : null,
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new SSAOptimizations() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				
				new StackLayoutStage(),
				new PlatformIntrinsicTransformationStage(),
				new PlatformStubStage(),
				new LoopAwareBlockOrderStage(),
				//new SimpleTraceBlockOrderStage(),
				//new SimpleRegisterAllocatorStage(),
				new CodeGenerationStage(),
			});
		}

	}
}
