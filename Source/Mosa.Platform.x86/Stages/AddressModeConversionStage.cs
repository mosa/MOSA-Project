/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class AddressModeConversionStage : BaseTransformationStage
	{
		protected override void Run()
		{
			foreach (var block in BasicBlocks)
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
					if (!node.IsEmpty)
						if (node.OperandCount == 2 && node.ResultCount == 1)
							ThreeTwoAddressConversion(node);
		}

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="node">The conversion context.</param>
		private void ThreeTwoAddressConversion(InstructionNode node)
		{
			if (!(node.Instruction is X86Instruction))
				return;

			if (!(node.OperandCount >= 1 && node.ResultCount >= 1 && node.Result != node.Operand1))
				return;

			Operand result = node.Result;
			Operand operand1 = node.Operand1;

			node.Operand1 = result;

			var move = GetMove(result, operand1);
			var size = GetInstructionSize(result.Type);

			var newNode = new InstructionNode(move, result, operand1);
			newNode.Size = size;
			node.Previous.Insert(newNode);

			return;
		}
	}
}
