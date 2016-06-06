// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Utility.Aot
{
	/// <summary>
	/// Specializes <see cref="AotMethodCompiler"/> for AOT purposes.
	/// </summary>
	public sealed class AotMethodCompiler : BaseMethodCompiler
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AotMethodCompiler" /> class.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		public AotMethodCompiler(BaseCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, int threadID)
			: base(compiler, method, basicBlocks, threadID)
		{
			var compilerOptions = compiler.CompilerOptions;

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
				(compilerOptions.EnableOptimizations && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableOptimizations) ? new IROptimizationStage() : null,

				(compilerOptions.TwoPassOptimizationStages && compilerOptions.EnableOptimizations && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.TwoPassOptimizationStages && compilerOptions.EnableOptimizations && compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new IROptimizationStage() : null,

				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				new IRCleanupStage(),
				(compilerOptions.EnableInlinedMethods) ? new InlineEvaluationStage() : null,
				new PlatformStubStage(),
				new PlatformEdgeSplitStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new BlockOrderingStage(),
				new CodeGenerationStage(),
				new ProtectedRegionLayoutStage(),
			});
		}

		#endregion Construction
	}
}
