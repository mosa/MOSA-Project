// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Trace;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions are moved
	/// and a simple move propagation is performed as well.
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class SimpleDeadCodeRemovalStage : BaseTransformationStage
	{
		private bool changed;
		private TraceLog trace;

		private Counter IRInstructionRemovedCount = new Counter("X86.SimpleDeadCodeRemovalStage.IRInstructionRemoved");

		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Initialize()
		{
			Register(IRInstructionRemovedCount);
		}

		protected override void Setup()
		{
			trace = CreateTraceLog();
		}

		protected override void Run()
		{
			changed = true;

			while (changed)
			{
				changed = false;

				foreach (var block in BasicBlocks)
				{
					for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
					{
						if (node.IsEmpty)
							continue;

						// Remove Nop instructions
						if (node.Instruction == X86.Nop)
						{
							node.Empty();
							continue;
						}

						if (node.Instruction == X86.Mov32)
						{
							Move(node);

							if (!node.IsEmpty)
							{
								RemoveUseless(node);
							}
							continue;
						}

						if (node.Instruction.IsIOOperation
							|| node.Instruction.IsMemoryRead
							|| node.Instruction.IsMemoryWrite
							|| node.Instruction.HasUnspecifiedSideEffect)
							continue;

						if (node.Instruction.FlowControl == FlowControl.Call)
							continue;

						var x86instruction = node.Instruction as X86Instruction;

						if (x86instruction == null)
							continue;

						// a more complex analysis would tracks the flag usage down the basic block to determine if the flags are used
						if (x86instruction.IsCarryFlagModified
							|| x86instruction.IsOverflowFlagModified
							|| x86instruction.IsZeroFlagModified
							|| x86instruction.IsSignFlagModified
							|| x86instruction.IsParityFlagModified)
							continue;

						RemoveUseless(node);
					}
				}
			}
		}

		private void RemoveUseless(InstructionNode node)
		{
			// Remove instruction, if useless
			if (node.ResultCount == 1 && node.Result.Uses.Count == 0 && node.Result.IsVirtualRegister)
			{
				// Check is split child, if so check is parent in use (IR.Return for example)
				if (node.Result.HasLongParent && node.Result.LongParent.Uses.Count != 0)
					return;

				if (trace.Active) trace.Log("REMOVED:\t" + node);

				node.Empty();
				changed = true;
				IRInstructionRemovedCount++;
			}
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Move(InstructionNode node)
		{
			Debug.Assert(node.Instruction == X86.Mov32);

			var result = node.Result;
			var source = node.Operand1;

			if (!result.IsVirtualRegister)
				return;

			if (!source.IsVirtualRegister)
				return;

			if (result.Definitions.Count != 1)
				return;

			if (source.Definitions.Count != 1)
				return;

			if (source.IsResolvedConstant)
				return;

			Debug.Assert(result != source);

			changed = true;

			foreach (var useNode in result.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (result == operand)
					{
						if (trace.Active) trace.Log("*** SimpleForwardCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}
			}

			Debug.Assert(result.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			node.Empty();
			IRInstructionRemovedCount++;
		}
	}
}
