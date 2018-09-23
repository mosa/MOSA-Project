// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Sparse Conditional Constant Propagation Stage
	/// </summary>
	public class SparseConditionalConstantPropagationStage : BaseMethodCompilerStage
	{
		protected TraceLog trace;

		protected int conditionalConstantPropagation = 0;
		protected int instructionsRemovedCount = 0;

		protected bool changed = false;

		protected override void Setup()
		{
			trace = CreateTraceLog();

			conditionalConstantPropagation = 0;
			instructionsRemovedCount = 0;
			changed = false;
		}

		protected override void Run()
		{
			var analysis = new SparseConditionalConstantPropagation(BasicBlocks, this);

			var deadBlocks = analysis.GetDeadBlocked();
			var constants = analysis.GetIntegerConstants();

			RemoveDeadBlocks(deadBlocks);
			ReplaceVirtualRegistersWithConstants(constants);

			UpdateCounter("ConditionalConstantPropagation.ConstantVariables", constants.Count);
			UpdateCounter("ConditionalConstantPropagation.ConstantPropagations", conditionalConstantPropagation);
			UpdateCounter("ConditionalConstantPropagation.DeadBlocks", deadBlocks.Count);
			UpdateCounter("ConditionalConstantPropagation.IRInstructionRemoved", instructionsRemovedCount);
		}

		protected override void Finish()
		{
			trace = null;
		}

		protected void ReplaceVirtualRegistersWithConstants(List<Tuple<Operand, ulong>> constantVirtualRegisters)
		{
			foreach (var value in constantVirtualRegisters)
			{
				ReplaceVirtualRegisterWithConstant(value.Item1, value.Item2);
			}
		}

		protected void ReplaceVirtualRegisterWithConstant(Operand target, ulong value)
		{
			if (trace.Active) trace.Log(target + " = " + value.ToString() + " Uses: " + target.Uses.Count.ToString());

			Debug.Assert(target.Definitions.Count == 1);

			if (target.Uses.Count != 0)
			{
				var constant = CreateConstant(target.Type, value);

				// for each statement T that uses operand, substituted c in statement T
				foreach (var node in target.Uses.ToArray())
				{
					Debug.Assert(node.Instruction != IRInstruction.AddressOf);

					for (int i = 0; i < node.OperandCount; i++)
					{
						var operand = node.GetOperand(i);

						if (operand != target)
							continue;

						if (trace.Active) trace.Log("*** ConditionalConstantPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + node);
						node.SetOperand(i, constant);
						conditionalConstantPropagation++;
						if (trace.Active) trace.Log("AFTER: \t" + node);

						changed = true;
					}
				}
			}

			Debug.Assert(target.Uses.Count == 0);

			if (target.Definitions.Count == 0)
				return;

			var defNode = target.Definitions[0];

			if (trace.Active) trace.Log("REMOVED:\t" + defNode);
			defNode.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		protected void RemoveDeadBlocks(List<BasicBlock> deadBlocks)
		{
			foreach (var block in deadBlocks)
			{
				RemoveBranchesToDeadBlocks(block);
			}

			//foreach (var block in deadBlocks)
			//{
			//	RemoveDeadBlock(block);
			//}
		}

		//protected void RemoveDeadBlock(BasicBlock deadBlock)
		//{
		//	if (trace.Active) trace.Log("*** RemoveBlock: " + deadBlock);

		//	var nextBlocks = deadBlock.NextBlocks.ToArray();

		//	EmptyBlockOfAllInstructions(deadBlock);

		//	RemoveBlockFromPhiInstructions(deadBlock, nextBlocks);

		//	Debug.Assert(deadBlock.NextBlocks.Count == 0);
		//	Debug.Assert(deadBlock.PreviousBlocks.Count == 0);
		//}

		protected void RemoveBranchesToDeadBlocks(BasicBlock deadBlock)
		{
			foreach (var previous in deadBlock.PreviousBlocks.ToArray())
			{
				// unable to handle more than two branch blocks
				// and if only one branch, then this block is dead as well
				if (previous.NextBlocks.Count != 2)
					return;

				var otherBlock = previous.NextBlocks[0] == deadBlock ? previous.NextBlocks[1] : previous.NextBlocks[0];

				for (var node = previous.Last.Previous; !node.IsBlockStartInstruction; node = node.Previous)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.BranchTargetsCount == 0)
						continue;

					if (node.Instruction == IRInstruction.CompareIntBranch32 || node.Instruction == IRInstruction.CompareIntBranch64)
					{
						if (trace.Active) trace.Log("*** RemoveBranchesToDeadBlocks");
						if (trace.Active) trace.Log("REMOVED:\t" + node);
						node.SetInstruction(IRInstruction.Nop);
						instructionsRemovedCount++;
					}
					else if (node.Instruction == IRInstruction.Jmp)
					{
						if (trace.Active) trace.Log("*** RemoveBranchesToDeadBlocks");
						if (trace.Active) trace.Log("BEFORE:\t" + node);
						node.UpdateBranchTarget(0, otherBlock);
						if (trace.Active) trace.Log("AFTER: \t" + node);
					}
				}
			}

			CheckAndClearEmptyBlock(deadBlock);
		}

		private void CheckAndClearEmptyBlock(BasicBlock block)
		{
			if (block.PreviousBlocks.Count != 0 || block.IsHeadBlock)
				return;

			if (trace.Active) trace.Log("*** RemoveBlock: " + block);

			var nextBlocks = block.NextBlocks.ToArray();

			EmptyBlockOfAllInstructions(block);

			_RemoveBlockFromPhiInstructions(block, nextBlocks);

			Debug.Assert(block.NextBlocks.Count == 0);
			Debug.Assert(block.PreviousBlocks.Count == 0);

			foreach (var next in nextBlocks)
			{
				CheckAndClearEmptyBlock(next);
			}
		}

		protected void _RemoveBlockFromPhiInstructions(BasicBlock removedBlock, BasicBlock[] nextBlocks)
		{
			foreach (var next in nextBlocks)
			{
				for (var node = next.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						break;

					var sourceBlocks = node.PhiBlocks;

					int index = sourceBlocks.IndexOf(removedBlock);

					if (index < 0)
						continue;

					sourceBlocks.RemoveAt(index);

					for (int i = index; i < node.OperandCount - 1; i++)
					{
						node.SetOperand(i, node.GetOperand(i + 1));
					}

					node.SetOperand(node.OperandCount - 1, null);
					node.OperandCount--;
				}
			}

			Debug.Assert(removedBlock.NextBlocks.Count == 0);
		}
	}
}
