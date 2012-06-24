/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System.Diagnostics;
using Mosa.Compiler.Framework.IR;

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
				for (var context = new Context(instructionSet, block); !context.EndOfInstruction; context.GotoNext())
				{
					if (context.Instruction is IR.Phi)
						ProcessPhiInstruction(block, context);

					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);
						if (op != null && op.IsSSA)
							context.SetOperand(i, op.SsaOperand);
					}

					if (context.Result != null)
					{
						if (context.Result.IsSSA)
							context.Result = context.Result.SsaOperand;
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
			var context = new Context(instructionSet, predecessor);
			while (!context.EndOfInstruction && IsBranchInstruction(context))
				context.GotoNext();

			if (context.Index != -1)
				context = context.InsertBefore();

			var source = operand.IsSSA ? operand.SsaOperand : operand;
			var destination = result.IsSSA ? result.SsaOperand : result;

			Debug.Assert(!source.IsSSA);
			Debug.Assert(!destination.IsSSA);

			if (destination != source)
				context.SetInstruction(IR.IRInstruction.Move, destination, source);
		}

		/// <summary>
		/// Determines whether [is branch instruction] [the specified context].
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		///   <c>true</c> if [is branch instruction] [the specified context]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsBranchInstruction(Context context)
		{
			return context.Instruction is Jmp || context.Instruction is IntegerCompareBranch;
		}

	}
}
