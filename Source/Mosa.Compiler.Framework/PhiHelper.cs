// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	public static class PhiHelper
	{
		public static void RemoveBlockFromPhi(BasicBlock removedBlock, BasicBlock phiBlock)
		{
			for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!node.Instruction.IsPhi)
					break;

				var sourceBlocks = node.PhiBlocks;

				var index = sourceBlocks.IndexOf(removedBlock);

				if (index < 0)
					continue;

				sourceBlocks.RemoveAt(index);

				for (var i = index; index < node.OperandCount - 1; index++)
				{
					node.SetOperand(i, node.GetOperand(i + 1));
				}

				node.SetOperand(node.OperandCount - 1, null);
				node.OperandCount--;
			}
		}

		public static void UpdatePhi(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != node.Block.PreviousBlocks.Count);

			// One or more of the previous blocks was removed, fix up the operand blocks

			var previousBlocks = node.Block.PreviousBlocks;
			var phiBlocks = node.PhiBlocks;

			for (var index = 0; index < node.OperandCount; index++)
			{
				var phiBlock = phiBlocks[index];

				if (previousBlocks.Contains(phiBlock))
					continue;

				phiBlocks.RemoveAt(index);

				for (var i = index; index < node.OperandCount - 1; index++)
				{
					node.SetOperand(i, node.GetOperand(i + 1));
				}

				node.SetOperand(node.OperandCount - 1, null);
				node.OperandCount--;

				index--;
			}

			Debug.Assert(node.OperandCount == node.Block.PreviousBlocks.Count);
		}

		public static void UpdatePhiBlock(BasicBlock phiBlock)
		{
			for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!node.Instruction.IsPhi)
					break;

				UpdatePhi(node);
			}
		}

		public static void UpdatePhiBlocks(BasicBlock[] phiBlocks)
		{
			foreach (var phiBlock in phiBlocks)
			{
				UpdatePhiBlock(phiBlock);
			}
		}

		public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
		{
			foreach (var phiBlock in phiBlocks)
			{
				UpdatePhiBlock(phiBlock);
			}
		}

		public static void UpdatePhiTarget(BasicBlock phiBlock, BasicBlock source, BasicBlock newSource)
		{
			Debug.Assert(phiBlock.PreviousBlocks.Count > 0);

			for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!node.Instruction.IsPhi)
					break;

				var index = node.PhiBlocks.IndexOf(source);

				//Debug.Assert(index >= 0);

				node.PhiBlocks[index] = newSource;
			}
		}

		public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
		{
			foreach (var target in targets)
			{
				UpdatePhiTarget(target, source, newSource);
			}
		}
	}
}
