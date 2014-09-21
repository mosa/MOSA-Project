/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class LeaveSSA : BaseMethodCompilerStage
	{
		private Dictionary<Operand, Operand> finalVirtualRegisters;

		protected override void Run()
		{
			if (!HasCode)
				return;

			if (HasProtectedRegions)
				return;

			finalVirtualRegisters = new Dictionary<Operand, Operand>();

			foreach (var block in BasicBlocks)
			{
				for (var context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					instructionCount++;

					if (context.Instruction == IRInstruction.Phi)
					{
						//Debug.Assert(context.OperandCount == context.BasicBlock.PreviousBlocks.Count);
						if (context.OperandCount != context.BasicBlock.PreviousBlocks.Count)
							throw new Mosa.Compiler.Common.InvalidCompilerException(context.ToString());

						ProcessPhiInstruction(context);
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

		protected override void Finish()
		{
			UpdateCounter("LeaveSSA.IRInstructions", instructionCount);
		}

		private Operand GetFinalVirtualRegister(Operand operand)
		{
			Operand final;

			if (!finalVirtualRegisters.TryGetValue(operand, out final))
			{
				if (operand.SSAVersion == 0)
					final = operand.SSAParent;
				else
					final = MethodCompiler.CreateVirtualRegister(operand.Type);

				finalVirtualRegisters.Add(operand, final);
			}

			return final;
		}

		/// <summary>
		/// Processes the phi instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ProcessPhiInstruction(Context context)
		{
			var sourceBlocks = context.Other as List<BasicBlock>;

			for (var index = 0; index < context.BasicBlock.PreviousBlocks.Count; index++)
			{
				var operand = context.GetOperand(index);
				var predecessor = sourceBlocks[index];

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
			var context = new Context(InstructionSet, predecessor, predecessor.EndIndex);

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
				context.AppendInstruction(IRInstruction.Move, destination, source);
			}
		}
	}
}