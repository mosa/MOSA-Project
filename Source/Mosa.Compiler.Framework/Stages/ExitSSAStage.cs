// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
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

			//if (HasProtectedRegions)
			//	return;

			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					InstructionCount++;

					if (!IsPhiInstruction(node.Instruction))
						break;

					if (node.OperandCount != node.Block.PreviousBlocks.Count)
						throw new CompilerException($"ExitSSAStage: Invalid block counts: {node}");

					ProcessPhiInstruction(node);
				}
			}

			MethodCompiler.IsInSSAForm = false;
		}

		/// <summary>
		/// Processes the phi instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void ProcessPhiInstruction(InstructionNode node)
		{
			var sourceBlocks = node.PhiBlocks;

			for (var index = 0; index < node.Block.PreviousBlocks.Count; index++)
			{
				var operand = node.GetOperand(index);
				var predecessor = sourceBlocks[index];

				InsertCopyStatement(predecessor, node.Result, operand);
			}

			node.Empty();
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

			while (node.Instruction != IRInstruction.Jmp)
			{
				node = node.Previous;
			}

			node = node.Previous;

			Debug.Assert(node.Instruction != IRInstruction.Branch32);
			Debug.Assert(node.Instruction != IRInstruction.Branch64);
			Debug.Assert(node.Instruction != IRInstruction.BranchObject);

			var context = new Context(node);

			if (!MosaTypeLayout.CanFitInRegister(destination))
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
