// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	internal class SimMethodCompiler : BaseMethodCompiler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SimMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		/// <param name="simAdapter">The sim adapter.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public SimMethodCompiler(SimCompiler compiler, MosaMethod method, ISimAdapter simAdapter, BasicBlocks basicBlocks, int threadID)
			: base(compiler, method, basicBlocks, threadID)
		{
			var compilerOptions = Compiler.CompilerOptions;

			// Populate the pipeline
			Pipeline.Add(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StackSetupStage(),
				new CILProtectedRegionStage(),
				new ExceptionStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new ConvertCompoundStage(),
				new UnboxValueTypeStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineStage() : null,
				(compilerOptions.EnableVariablePromotion) ? new PromoteTempVariablesStage() : null,
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableOptimizations && compilerOptions.EnableSSA && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableOptimizations) ? new IROptimizationStage() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				new IRCleanupStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineEvaluationStage() : null,
				new PlatformStubStage(),
				new PlatformEdgeSplitStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new BlockOrderingStage(),
				new SimCodeGeneratorStage(simAdapter),
				new ProtectedRegionLayoutStage(),
			});
		}
	}
}
