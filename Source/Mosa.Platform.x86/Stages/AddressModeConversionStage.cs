// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Address Mode Conversion Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class AddressModeConversionStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			// Nothing to do
		}

		protected override void Run()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (!node.IsEmpty && node.OperandCount == 2 && node.ResultCount == 1)
					{
						ThreeTwoAddressConversion(node);
					}
				}
			}
		}

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="node">The conversion context.</param>
		private void ThreeTwoAddressConversion(InstructionNode node)
		{
			var instruction = node.Instruction as X86Instruction;

			if (instruction == null)
				return;

			if (!instruction.ThreeTwoAddressConversion)
				return;

			if (!(node.OperandCount >= 1 && node.ResultCount >= 1 && node.Result != node.Operand1))
				return;

			var result = node.Result;
			var operand1 = node.Operand1;
			int label = node.Label;

			node.Operand1 = result;

			X86Instruction move = null;
			var size = InstructionSize.None;

			if (result.Type.IsR4)
			{
				move = X86.Movss;
				size = InstructionSize.Size32;
			}
			else if (result.Type.IsR8)
			{
				move = X86.Movsd;
				size = InstructionSize.Size64;
			}
			else
			{
				move = X86.Mov32;
				size = InstructionSize.Size32;
			}

			var newNode = new InstructionNode(move, result, operand1)
			{
				Size = size,
				Label = label
			};
			node.Previous.Insert(newNode);
		}
	}
}
