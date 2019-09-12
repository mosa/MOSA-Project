// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Leave SSA Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseMethodCompilerStage" />
	public class ExitSSAStage : BaseMethodCompilerStage
	{
		private Counter InstructionCount = new Counter("ExitSSAStage.IRInstructions");

		protected override void Initialize()
		{
			Register(InstructionCount);
		}

		protected override void Run()
		{
			if (!HasCode)
				return;

			if (HasProtectedRegions)
				return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					InstructionCount++;

					if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
						break;

					Debug.Assert(node.OperandCount == node.Block.PreviousBlocks.Count);

					ProcessPhiInstruction(node);
				}
			}
		}

		/// <summary>
		/// Processes the phi instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void ProcessPhiInstruction(InstructionNode node)
		{
			//if (SimplePhiUpdate(node))
			//	return;

			var sourceBlocks = node.PhiBlocks;

			for (var index = 0; index < node.Block.PreviousBlocks.Count; index++)
			{
				var operand = node.GetOperand(index);
				var predecessor = sourceBlocks[index];

				InsertCopyStatement(predecessor, node.Result, operand);
			}

			node.Empty();
		}

		private bool SimplePhiUpdate(InstructionNode node)
		{
			// Experiment
			var result = node.Result;

			if (result.Definitions.Count != 1)
				return false;

			for (int i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (operand.IsConstant)
					continue;

				if (operand.Definitions.Count != 1)
					return false;

				if (!operand.IsVirtualRegister)
					return false;

				if (operand.Definitions[0].Block != node.PhiBlocks[i])
					return false;
			}

			for (int i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (operand.IsVirtualRegister)
				{
					operand.Definitions[0].Result = result;

					ReplaceOperand(operand, result);
				}
				else
				{
					InsertCopyStatement(node.PhiBlocks[i], result, operand);
				}
			}

			node.Empty();

			return true;
		}

		/// <summary>
		/// Inserts the copy statement.
		/// </summary>
		/// <param name="predecessor">The predecessor.</param>
		/// <param name="destination">The result.</param>
		/// <param name="source">The operand.</param>
		private void InsertCopyStatement(BasicBlock predecessor, Operand destination, Operand source)
		{
			if (destination == source)
				return;

			var node = predecessor.BeforeLast;

			while (node.IsEmptyOrNop
				|| node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64
				|| node.Instruction == IRInstruction.Jmp)
			{
				node = node.Previous;
			}

			var context = new Context(node);

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
