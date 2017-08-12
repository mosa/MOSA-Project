// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Leave SSA Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public class LeaveSSAStage : BaseMethodCompilerStage
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
				for (var context = new Context(block); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					instructionCount++;

					if (context.Instruction == IRInstruction.Phi)
					{
						//Debug.Assert(context.OperandCount == context.BasicBlock.PreviousBlocks.Count);
						if (context.OperandCount != context.Block.PreviousBlocks.Count)
						{
							throw new Mosa.Compiler.Common.InvalidCompilerException(context.ToString());
						}

						ProcessPhiInstruction(context);
					}

					for (var i = 0; i < context.OperandCount; ++i)
					{
						var op = context.GetOperand(i);

						if (op?.IsSSA == true)
						{
							context.SetOperand(i, GetFinalVirtualRegister(op));
						}
					}

					if (context.Result?.IsSSA == true)
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
			if (!finalVirtualRegisters.TryGetValue(operand, out Operand final))
			{
				if (operand.SSAVersion == 0)
					final = operand.SSAParent;
				else
					final = AllocateVirtualRegister(operand.Type);

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
			var sourceBlocks = context.PhiBlocks;

			for (var index = 0; index < context.Block.PreviousBlocks.Count; index++)
			{
				var operand = context.GetOperand(index);
				var predecessor = sourceBlocks[index];

				InsertCopyStatement(predecessor, context.Result, operand);
			}

			context.Empty();
		}

		/// <summary>
		/// Inserts the copy statement.
		/// </summary>
		/// <param name="predecessor">The predecessor.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand">The operand.</param>
		private void InsertCopyStatement(BasicBlock predecessor, Operand result, Operand operand)
		{
			var context = new Context(predecessor.Last);

			context.GotoPrevious();

			while (context.IsEmpty
				|| context.Instruction is CompareIntegerBranch
				|| context.Instruction is Jmp)
			{
				context.GotoPrevious();
			}

			var source = operand.IsSSA ? GetFinalVirtualRegister(operand) : operand;
			var destination = result.IsSSA ? GetFinalVirtualRegister(result) : result;

			Debug.Assert(!source.IsSSA);
			Debug.Assert(!destination.IsSSA);

			if (destination != source)
			{
				if (MosaTypeLayout.IsStoredOnStack(destination.Type))
				{
					context.AppendInstruction(IRInstruction.MoveCompound, destination, source);
					context.MosaType = destination.Type;
				}
				else
				{
					var moveInstruction = GetMoveInstruction(destination.Type);
					context.AppendInstruction(moveInstruction, destination, source);
				}
			}
		}
	}
}
