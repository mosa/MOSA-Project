/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.CIL;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class LeaveSSA : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			foreach (var block in basicBlocks)
			{
				for (var context = new Context(instructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.Instruction is IR.Phi)
					{
						ProcessPhiInstruction(block, context);
					}

					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);
						if (op != null && op.IsSSA)
						{
							context.SetOperand(i, op.SSAParent);
						}
					}

					if (context.Result != null && context.Result.IsSSA)
					{
						context.Result = context.Result.SSAParent;
					}

				}
			}
		}

		/// <summary>
		/// Processes the phi instruction.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="context">The context.</param>
		private void ProcessPhiInstruction(BasicBlock block, Context context)
		{
			for (var index = 0; index < block.PreviousBlocks.Count; index++)
			{
				var predecessor = block.PreviousBlocks[index];
				var operand = context.GetOperand(index);

				InsertCopyStatement(predecessor, context.Result, operand);
			}
			context.Remove();
		}

		/// <summary>
		/// Inserts the copy statement.
		/// </summary>
		/// <param name="predecessor">The predecessor.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand">The operand.</param>
		private void InsertCopyStatement(BasicBlock predecessor, Operand result, Operand operand)
		{
			var context = new Context(instructionSet, predecessor, predecessor.EndIndex);

			context.GotoPrevious();

			while (context.Instruction is IntegerCompareBranch || context.Instruction is Jmp)
			{
				context.GotoPrevious();
			}

			var source = operand.IsSSA ? operand.SSAParent : operand;
			var destination = result.IsSSA ? result.SSAParent : result;

			Debug.Assert(!source.IsSSA);
			Debug.Assert(!destination.IsSSA);

			if (destination != source)
				context.AppendInstruction(IRInstruction.Move, destination, source);
		}

	}
}