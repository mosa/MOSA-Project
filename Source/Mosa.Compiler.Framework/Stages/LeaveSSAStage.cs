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
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class LeaveSSA : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		private Dictionary<Operand, Operand> finalVirtualRegisters;

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			finalVirtualRegisters = new Dictionary<Operand, Operand>();

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
							context.SetOperand(i, GetFinalVirtualRegister(op));
						}
					}

					if (context.Result != null && context.Result.IsSSA)
					{
						context.Result = GetFinalVirtualRegister(context.Result);
					}
				}
			}

			finalVirtualRegisters = null;
		}

		private Operand GetFinalVirtualRegister(Operand operand)
		{
			Operand final;

			if (!finalVirtualRegisters.TryGetValue(operand, out final))
			{
				if (operand.SSAVersion == 0)
					final = operand.SSAParent;
				else
					final = methodCompiler.CreateVirtualRegister(operand.Type);

				finalVirtualRegisters.Add(operand, final);
			}

			return final;
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

			while (context.IsEmpty || context.Instruction is IntegerCompareBranch || context.Instruction is Jmp)
			{
				context.GotoPrevious();
			}

			var source = operand.IsSSA ? GetFinalVirtualRegister(operand) : operand;
			var destination = result.IsSSA ? GetFinalVirtualRegister(result) : result;

			Debug.Assert(!source.IsSSA);
			Debug.Assert(!destination.IsSSA);

			if (destination != source)
			{
				architecture.InsertMoveInstruction(context, destination, source);
			}
		}
	}
}