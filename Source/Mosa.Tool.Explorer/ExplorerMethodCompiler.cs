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
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer
{
	internal class ExplorerMethodCompiler : BaseMethodCompiler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExplorerMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="threadID">The thread identifier.</param>
		public ExplorerMethodCompiler(ExplorerCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, int threadID)
			: base(compiler, method, basicBlocks, threadID)
		{
			var compilerOptions = Compiler.CompilerOptions;

			// Populate the pipeline
			Pipeline.Add(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new ExceptionPrologueStage(),

				new OperandAssignmentStage(),
				new StackSetupStage(),
				new ProtectedRegionStage(),

				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new ConvertCompoundStage(),
				new UnboxValueTypeStage(),
				new ExceptionStage(),

				new InlineStage(),
				new StopStage(),

				(compilerOptions.EnablePromoteTemporaryVariablesOptimization) ? new PromoteTempVariablesStage() : null,

				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,

				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableOptimizations) ? new IROptimizationStage() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,

				new IRCleanup(),
				new InlineEvaluationStage(),
				new PlatformStubStage(),
				new	PlatformEdgeSplitStage(),

				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),

				new EmptyBlockRemovalStage(),
				new BlockOrderingStage(),
				new CodeGenerationStage(compilerOptions.EmitBinary),
				(compilerOptions.EmitBinary) ? new ProtectedRegionLayoutStage() : null
			});
		}
	}
}
