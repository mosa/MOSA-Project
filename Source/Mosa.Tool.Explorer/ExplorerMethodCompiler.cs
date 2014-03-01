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
		/// <param name="emitBinary">if set to <c>true</c> [emit binary].</param>
		public ExplorerMethodCompiler(ExplorerCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet, bool emitBinary)
			: base(compiler, method, basicBlocks, instructionSet)
		{
			var compilerOptions = Compiler.CompilerOptions;

			// Populate the pipeline
			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new StackSetupStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				//new SingleUseMarkerStage(),
				//new OperandUsageAnalyzerStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new ConvertCompoundMoveStage(),
				//new CheckIROperandCountStage(),
				(compilerOptions.EnableSSA) ? new PromoteLocalVariablesStage() : null,
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new SSAOptimizations() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				(compilerOptions.EnableSSA) ? new ConvertCompoundMoveStage() : null,
				new PlatformStubStage(),
				//new CheckPlatformOperandCountStage(),
				new	PlatformEdgeSplitStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new LoopAwareBlockOrderStage(),
				new CodeGenerationStage(emitBinary),
			});
		}
	}
}