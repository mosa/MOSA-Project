// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

// FIXME: This stage depends on Node.Next & Node.Previous properties but the compiler may insert empty nodes.
// Rewrite not to depend on those properties.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage identifies move/load/store/call instructions with compound types
	/// (i.e. user defined value types with size greater than sans enums) operands and
	/// convert them into respective compound instructions, which will be expanded into
	/// multiple native instructions by platform layer.
	/// </summary>
	public class ConvertCompoundStage : BaseMethodCompilerStage
	{
		private Dictionary<Operand, Operand> repl = new Dictionary<Operand, Operand>();

		protected override void Run()
		{
			for (int index = 0; index < BasicBlocks.Count; index++)
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
					if (!node.IsEmpty)
						ProcessInstruction(node);

			for (int index = 0; index < BasicBlocks.Count; index++)
				for (var node = BasicBlocks[index].First; !node.IsBlockEndInstruction; node = node.Next)
					if (!node.IsEmpty)
						ReplaceOperands(node);
		}

		protected override void Finish()
		{
			repl = null;
		}

		private void ProcessInstruction(InstructionNode node)
		{
			if (node.Instruction == IRInstruction.Load)
			{
				if (node.MosaType != null && TypeLayout.IsCompoundType(node.MosaType))
				{
					if (node.Result.IsVirtualRegister && !repl.ContainsKey(node.Result))
					{
						repl[node.Result] = MethodCompiler.StackLayout.AddStackLocal(node.MosaType);
					}
					node.ReplaceInstructionOnly(IRInstruction.CompoundLoad);
				}
			}
			else if (node.Instruction == IRInstruction.Store)
			{
				if (node.MosaType != null && TypeLayout.IsCompoundType(node.MosaType))
				{
					if (node.Operand3.IsVirtualRegister && !repl.ContainsKey(node.Operand3))
					{
						repl[node.Operand3] = MethodCompiler.StackLayout.AddStackLocal(node.MosaType);
					}
					node.ReplaceInstructionOnly(IRInstruction.CompoundStore);
				}
			}
			else if (node.Instruction == IRInstruction.Move)
			{
				if (node.Result.Type.Equals(node.Operand1.Type) && TypeLayout.IsCompoundType(node.Result.Type))
				{
					var prevNode = node.Previous;
					var nextNode = node.Next;
					while (prevNode.IsEmpty)
						prevNode = prevNode.Previous;
					while (nextNode.IsEmpty)
						nextNode = nextNode.Next;

					// If this move is proceeded by a return then remove this instruction
					// It is basically a double up caused by some instructions result in the same instruction output
					if (nextNode.Instruction == IRInstruction.Return && nextNode.Operand1 == node.Result)
					{
						nextNode.Operand1 = node.Operand1;

						node.Empty();
						return;
					}

					// If this move is preceded by a compound move (which will turn into a compound move) remove this instruction
					// It is basically a double up caused by some instructions result in the same IR output
					if ((prevNode.Instruction == IRInstruction.CompoundMove
							|| prevNode.Instruction == IRInstruction.CompoundLoad
							|| prevNode.Instruction == IRInstruction.Call)
						&& prevNode.Result == node.Operand1)
					{
						if (repl.ContainsKey(prevNode.Result))
							repl[node.Result] = repl[prevNode.Result];
						prevNode.Result = node.Result;

						node.Empty();
						return;
					}

					if (node.Result.IsVirtualRegister && !repl.ContainsKey(node.Result))
					{
						repl[node.Result] = MethodCompiler.StackLayout.AddStackLocal(node.Result.Type);
					}
					if (node.Operand1.IsVirtualRegister && !repl.ContainsKey(node.Operand1))
					{
						repl[node.Operand1] = MethodCompiler.StackLayout.AddStackLocal(node.Operand1.Type);
					}
					node.ReplaceInstructionOnly(IRInstruction.CompoundMove);
				}
				else if (node.Result.Type.Equals(node.Operand1.Type) && node.Result.IsStackLocal && node.Operand1.IsStackLocal)
				{
					node.ReplaceInstructionOnly(IRInstruction.CompoundMove);
				}
			}
			else if (node.Instruction == IRInstruction.Call)
			{
				if (node.Result != null && TypeLayout.IsCompoundType(node.Result.Type))
				{
					if (node.Result.IsVirtualRegister && !repl.ContainsKey(node.Result))
					{
						repl[node.Result] = MethodCompiler.StackLayout.AddStackLocal(node.Result.Type);
					}
				}
			}
		}

		private void ReplaceOperands(InstructionNode node)
		{
			if (node.Result != null && repl.ContainsKey(node.Result))
				node.Result = repl[node.Result];

			if (node.Result2 != null && repl.ContainsKey(node.Result2))
				node.Result2 = repl[node.Result2];

			int count = node.OperandCount;
			for (int i = 0; i < count; i++)
			{
				var operand = node.GetOperand(i);
				if (operand != null && repl.ContainsKey(operand))
					node.SetOperand(i, repl[operand]);
			}
		}
	}
}
