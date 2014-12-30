/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
		/// <param name="instructionSet">The instruction set.</param>
		/// <param name="threadID"></param>
		public AotMethodCompiler(BaseCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet, int threadID)
			: base(compiler, method, basicBlocks, instructionSet, threadID)
		{
			var compilerOptions = compiler.CompilerOptions;

			// Populate the pipeline
			Pipeline.Add(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StackSetupStage(),
				new ProtectedRegionStage(),
				//new ProtectedRegionFlowUpdateStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new ConvertCompoundStage(),
				new UnboxValueTypeStage(),
				new ExceptionStage(),

				(compilerOptions.EnablePromoteTemporaryVariablesOptimization) ? new PromoteTempVariablesStage() : null,

				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSparseConditionalConstantPropagation && compilerOptions.EnableSSA) ? new SparseConditionalConstantPropagationStage() : null,
				(compilerOptions.EnableOptimizations) ? new IROptimizationStage() : null,
				//(compilerOptions.EnableSSA) ? new DeadCodeRemovalStage() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,

				new IRCleanup(),
				new PlatformStubStage(),
				new	PlatformEdgeSplitStage(),
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
