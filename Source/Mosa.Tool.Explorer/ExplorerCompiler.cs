// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.CompilerStages;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Tool.Explorer
{
	internal class ExplorerCompiler : BaseCompiler
	{
		/// <summary>
		/// Extends the compiler setup.
		/// </summary>
		public override void ExtendCompilerSetup()
		{
			CompilePipeline.Add(new BaseCompilerStage[] {
				new PlugStage(),
				new TypeInitializerSchedulerStage(),
				new MethodLookupTableStage(),
				new MethodExceptionLookupTableStage(),
				new MetadataStage(),
			});
		}

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="method">The method to compile.</param>
		/// <param name="basicBlocks">The basic blocks.</param>
		/// <param name="threadID">The thread identifier.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		protected override MethodCompiler CreateMethodCompiler(MosaMethod method, BasicBlocks basicBlocks, int threadID)
		{
			var methodcompiler = new MethodCompiler(this, method, basicBlocks, threadID);

			// Populate the pipeline
			methodcompiler.Pipeline.Add(new BaseMethodCompilerStage[] {
				new CILDecodingStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StackSetupStage(),
				new CILProtectedRegionStage(),
				new ExceptionStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new UnboxValueTypeStage(),
				(CompilerOptions.EnableInlinedMethods) ? new InlineStage() : null,
				(CompilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(CompilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(CompilerOptions.EnableSparseConditionalConstantPropagation && CompilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(CompilerOptions.EnableIROptimizations) ? new IROptimizationStage() : null,
				new LowerIRStage(),
				(CompilerOptions.IRLongExpansion && Architecture.NativePointerSize == 4) ? new IRLongExpansionStage() : null,
				(CompilerOptions.TwoPassOptimizations && CompilerOptions.EnableIROptimizations && CompilerOptions.EnableSparseConditionalConstantPropagation && CompilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(CompilerOptions.TwoPassOptimizations && CompilerOptions.EnableIROptimizations && CompilerOptions.EnableSparseConditionalConstantPropagation && CompilerOptions.EnableSSA) ? new IROptimizationStage() : null,
				(CompilerOptions.EnableSSA) ? new LeaveSSAStage() : null,
				new IRCleanupStage(),
				(CompilerOptions.EnableInlinedMethods) ? new InlineEvaluationStage() : null,
				new DevirtualizeCallStage(),
				new CallStage(),
				new PlatformStubStage(),
				new PlatformEdgeSplitStage(),
				new VirtualRegisterRenameStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new BlockOrderingStage(),
				new CodeGenerationStage(CompilerOptions.EmitBinary),
				new PreciseGCStage(),
				new GraphVizStage(),
				(CompilerOptions.EmitBinary) ? new ProtectedRegionLayoutStage() : null,
				(CompilerOptions.EmitBinary) ? new DisassemblyStage() : null
			});

			return methodcompiler;
		}
	}
}
