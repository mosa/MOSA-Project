// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

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
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					var instruction = node.Instruction as X86Instruction;

					if (instruction?.ThreeTwoAddressConversion == true)
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
			if (node.Result.IsCPURegister
				&& node.Operand1.IsCPURegister
				&& node.Result.Register == node.Operand1.Register)
				return;

			if (node.Result == node.Operand1)
				return;

			var result = node.Result;
			var operand1 = node.Operand1;
			int label = node.Label;

			node.Operand1 = result;

			X86Instruction move = null;

			if (result.Type.IsR4)
			{
				move = X86.Movss;
			}
			else if (result.Type.IsR8)
			{
				move = X86.Movsd;
			}
			else
			{
				move = X86.Mov32;
			}

			var newNode = new InstructionNode(move, result, operand1)
			{
				Label = label
			};

			node.Previous.Insert(newNode);
		}
	}
}
