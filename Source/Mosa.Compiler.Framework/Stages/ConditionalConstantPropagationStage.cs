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
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class ConditionalConstantPropagationStage : BaseMethodCompilerStage
	{
		protected SectionTrace trace;

		protected int conditionalConstantPropagation = 0;
		protected int instructionsRemovedCount = 0;

		protected bool changed = false;

		protected override void Setup()
		{
			trace = CreateTrace();
		}

		protected override void Run()
		{
			// FIXME - VERY TEMPORARY HACK!
			if (MethodCompiler.Method.FullName.Contains("ConsoleSession::.ctor(System.Byte"))
				return;

			var analysis = new ConditionalConstantPropagation(BasicBlocks, InstructionSet, trace);

			var deadBlocks = analysis.GetDeadBlocked();
			var constants = analysis.GetIntegerConstants();

			ReplaceVirtualRegistersWithConstants(constants);
			//RemoveDeadBlocks(deadBlocks);

			UpdateCounter("ConditionalConstantPropagation.ConstantVariableCount", constants.Count);
			UpdateCounter("ConditionalConstantPropagation.ConstantVariableUse", conditionalConstantPropagation);
			UpdateCounter("ConditionalConstantPropagation.IRInstructionRemoved", instructionsRemovedCount);

			if (changed)
			{
				this.MethodCompiler.InternalTrace.CompilerEventListener.SubmitTraceEvent(CompilerEvent.Special, "SCCP: " + MethodCompiler.Method.ToString());
			}
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

			if (target.Uses.Count == 0)
				return;

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

		protected void RemoveDeadBlocks(List<BasicBlock> blocks)
		{
			foreach (var block in blocks)
			{
				RemoveDeadBlockInstructions(block);
			}
		}

		protected void RemoveDeadBlockInstructions(BasicBlock block)
		{
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

				if (context.Instruction == IRInstruction.Jmp || context.Instruction == IRInstruction.IntegerCompareBranch)
					continue;

				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
			}
		}
	}
}