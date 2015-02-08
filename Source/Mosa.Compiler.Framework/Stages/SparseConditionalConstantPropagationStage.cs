/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
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
		}

		protected override void Run()
		{
			var analysis = new SparseConditionalConstantPropagation(BasicBlocks, InstructionSet, this);

			var deadBlocks = analysis.GetDeadBlocked();
			var constants = analysis.GetIntegerConstants();

			ReplaceVirtualRegistersWithConstants(constants);
			RemoveDeadBlocks(deadBlocks);

			UpdateCounter("ConditionalConstantPropagation.ConstantVariableCount", constants.Count);
			UpdateCounter("ConditionalConstantPropagation.ConstantVariableUse", conditionalConstantPropagation);
			UpdateCounter("ConditionalConstantPropagation.DeadBlocks", deadBlocks.Count);
			UpdateCounter("ConditionalConstantPropagation.IRInstructionRemoved", instructionsRemovedCount);
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
			if (trace.Active) trace.Log(target.ToString() + " = " + value.ToString() + " Uses: " + target.Uses.Count.ToString());

			Debug.Assert(target.Definitions.Count == 1);

			if (target.Uses.Count != 0)
			{
				var constant = Operand.CreateConstant(target.Type, value);

				// for each statement T that uses operand, substituted c in statement T
				foreach (int index in target.Uses.ToArray())
				{
					var context = new Context(InstructionSet, index);

					Debug.Assert(context.Instruction != IRInstruction.AddressOf);

					for (int i = 0; i < context.OperandCount; i++)
					{
						var operand = context.GetOperand(i);

						if (operand != target)
							continue;

						if (trace.Active) trace.Log("*** ConditionalConstantPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
						context.SetOperand(i, constant);
						conditionalConstantPropagation++;
						if (trace.Active) trace.Log("AFTER: \t" + context.ToString());

						changed = true;
					}
				}
			}

			Debug.Assert(target.Uses.Count == 0);

			if (target.Definitions.Count == 0)
				return;

			var ctx = new Context(InstructionSet, target.Definitions[0]);

			if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		protected void RemoveDeadBlocks(List<BasicBlock> blocks)
		{
			foreach (var block in blocks)
			{
				RemoveDeadBlock(block);
			}
		}

		protected void RemoveDeadBlock(BasicBlock block)
		{
			if (trace.Active) trace.Log("*** RemoveBlock: " + block.ToString());

			foreach (var prev in block.PreviousBlocks)
			{
				//Debug.Assert(prev.NextBlocks.Count <= 2);

				prev.NextBlocks.Remove(block);

				bool unconditional = false;

				var context = new Context(InstructionSet, block, prev.EndIndex);

				while (context.IsEmpty || context.IsBlockEndInstruction || context.Targets != null)
				{
					if (context.Instruction.FlowControl == FlowControl.ConditionalBranch ||
						context.Instruction.FlowControl == FlowControl.UnconditionalBranch ||
						context.Instruction.FlowControl == FlowControl.Switch)
					{
						Debug.Assert(context.Targets.Count == 1);

						var branch = context.Targets[0];

						if (branch == block)
						{
							unconditional = context.Instruction.FlowControl == FlowControl.UnconditionalBranch;

							if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
							context.SetInstruction(IRInstruction.Nop);
							instructionsRemovedCount++;
						}
						else
						{
							if (unconditional)
							{
								if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
								context.SetInstruction(IRInstruction.Jmp);
								context.AddBranch(branch);
								if (trace.Active) trace.Log("AFTER:\t" + context.ToString());
								unconditional = false;
							}
						}
					}

					context.GotoPrevious();
				}
			}

			block.PreviousBlocks.Clear();

			var nextBlocks = new List<BasicBlock>(block.NextBlocks.Count);

			foreach (var next in block.NextBlocks)
			{
				next.PreviousBlocks.Remove(block);
				nextBlocks.Add(next);
			}

			block.NextBlocks.Clear();

			for (var context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (context.IsEmpty)
					continue;

				if (context.IsBlockStartInstruction)
					continue;

				if (context.Instruction == IRInstruction.Nop)
					continue;

				if (context.Instruction == IRInstruction.Epilogue)
					continue;

				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
			}

			// Update PHI lists
			foreach (var next in nextBlocks)
			{
				for (var context = new Context(InstructionSet, next); !context.IsBlockEndInstruction; context.GotoNext())
				{
					if (context.IsEmpty)
						continue;

					if (context.Instruction != IRInstruction.Phi)
						continue;

					var sourceBlocks = context.PhiBlocks;

					int index = sourceBlocks.IndexOf(block);

					if (index < 0)
						continue;

					sourceBlocks.RemoveAt(index);

					for (int i = index; index < context.OperandCount - 1; index++)
					{
						context.SetOperand(i, context.GetOperand(i + 1));
					}

					context.SetOperand(context.OperandCount - 1, null);
					context.OperandCount--;
				}
			}
		}
	}
}