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

namespace Mosa.Tool.Compiler
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
		public AotMethodCompiler(BaseCompiler compiler, MosaMethod method, BasicBlocks basicBlocks, InstructionSet instructionSet)
			: base(compiler, method, basicBlocks, instructionSet)
		{
			var compilerOptions = compiler.CompilerOptions;

			Pipeline.AddRange(new IMethodCompilerStage[] {
				new CILDecodingStage(),
				new BasicBlockBuilderStage(),
				new StackSetupStage(),
				new ExceptionPrologueStage(),
				new OperandAssignmentStage(),
				new StaticAllocationResolutionStage(),
				new CILTransformationStage(),
				new ConvertCompoundMoveStage(),
				(compilerOptions.EnableSSA) ? new PromoteLocalVariablesStage() : null,
				(compilerOptions.EnableSSA) ? new EdgeSplitStage() : null,
				(compilerOptions.EnableSSA) ? new DominanceCalculationStage() : null,
				(compilerOptions.EnableSSA) ? new PhiPlacementStage() : null,
				(compilerOptions.EnableSSA) ? new EnterSSAStage() : null,
				(compilerOptions.EnableSSA && compilerOptions.EnableSSAOptimizations) ? new SSAOptimizations() : null,
				(compilerOptions.EnableSSA) ? new LeaveSSA() : null,
				(compilerOptions.EnableSSA) ? new ConvertCompoundMoveStage() : null,
				new PlatformStubStage(),
				new	PlatformEdgeSplitStage(),
				new GreedyRegisterAllocatorStage(),
				new StackLayoutStage(),
				new EmptyBlockRemovalStage(),
				new LoopAwareBlockOrderStage(),
				new CodeGenerationStage(),
			});
		}

		#endregion Construction
	}
}