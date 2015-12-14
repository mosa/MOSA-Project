// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			var analysis = new SparseConditionalConstantPropagation(BasicBlocks, this);

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
				foreach (var node in target.Uses.ToArray())
				{
					Debug.Assert(node.Instruction != IRInstruction.AddressOf);

					for (int i = 0; i < node.OperandCount; i++)
					{
						var operand = node.GetOperand(i);

						if (operand != target)
							continue;

						if (trace.Active) trace.Log("*** ConditionalConstantPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + node.ToString());
						node.SetOperand(i, constant);
						conditionalConstantPropagation++;
						if (trace.Active) trace.Log("AFTER: \t" + node.ToString());

						changed = true;
					}
				}
			}

			Debug.Assert(target.Uses.Count == 0);

			if (target.Definitions.Count == 0)
				return;

			var defNode = target.Definitions[0];

			if (trace.Active) trace.Log("REMOVED:\t" + defNode.ToString());
			defNode.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		protected void RemoveDeadBlocks(List<BasicBlock> deadBlocks)
		{
			foreach (var block in deadBlocks)
			{
				RemoveDeadBlock(block);
			}

			// FIXME: This may leave branch instructions pointing to a dead block
			// IR Optimization Stage --- does catch and clean this up,
			//    but only if optimizations are turned on enabled
		}

		protected void RemoveDeadBlock(BasicBlock block)
		{
			if (trace.Active) trace.Log("*** RemoveBlock: " + block.ToString());

			var nextBlocks = block.NextBlocks.ToArray();

			EmptyBlockOfAllInstructions(block);

			UpdatePhiList(block, nextBlocks);
		}
	}
}
