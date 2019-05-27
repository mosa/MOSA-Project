// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Framework.Trace;
using System.Diagnostics;

namespace Mosa.Platform.Intel.Stages
{
	/// <summary>
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions are moved
	/// and a simple move propagation is performed as well.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformTransformationStage" />
	public abstract class SimpleDeadCodeRemovalStage : BasePlatformTransformationStage
	{
		#region Abstract Methods

		protected abstract bool IsNop(BaseInstruction instruction);

		protected abstract bool IsMov(BaseInstruction instruction);

		#endregion Abstract Methods

		protected bool changed;
		protected TraceLog trace;

		protected Counter IRInstructionRemovedCount;

		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Initialize()
		{
			IRInstructionRemovedCount = new Counter(Platform + ".SimpleDeadCodeRemovalStage.IRInstructionRemoved");

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
						if (IsNop(node.Instruction))
						{
							node.Empty();
							continue;
						}

						if (IsMov(node.Instruction))
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

						var instruction = node.Instruction as BasePlatformInstruction;

						if (instruction == null)
							continue;

						// a more complex analysis would tracks the flag usage down the basic block to determine if the flags are used
						if (instruction.IsCarryFlagModified
							|| instruction.IsOverflowFlagModified
							|| instruction.IsZeroFlagModified
							|| instruction.IsSignFlagModified
							|| instruction.IsParityFlagModified)
							continue;

						RemoveUseless(node);
					}
				}
			}
		}

		protected void RemoveUseless(InstructionNode node)
		{
			// Remove instruction, if useless
			if (node.ResultCount == 1 && node.Result.Uses.Count == 0 && node.Result.IsVirtualRegister)
			{
				// Check is split child, if so check is parent in use (IR.Return for example)
				if (node.Result.HasLongParent && node.Result.LongParent.Uses.Count != 0)
					return;

				trace?.Log($"REMOVED:\t{node}");

				node.Empty();
				changed = true;
				IRInstructionRemovedCount++;
			}
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		protected void Move(InstructionNode node)
		{
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

			//ReplaceOperand(result, source);
			foreach (var useNode in result.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (result == operand)
					{
						trace?.Log("*** SimpleForwardCopyPropagation");
						trace?.Log($"BEFORE:\t{useNode}");
						useNode.SetOperand(i, source);
						trace?.Log($"AFTER: \t{useNode}");
					}
				}
			}

			Debug.Assert(result.Uses.Count == 0);

			trace?.Log($"REMOVED:\t{node}");
			node.Empty();
			IRInstructionRemovedCount++;
		}
	}
}
