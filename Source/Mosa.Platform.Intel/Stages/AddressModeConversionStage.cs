// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Platform.Intel.Stages
{
	/// <summary>
	/// Address Mode Conversion Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformTransformationStage" />
	public abstract class AddressModeConversionStage : BasePlatformTransformationStage
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
					if (node.IsEmptyOrNop)
						continue;

					if (IsThreeTwoAddressRequired(node.Instruction))
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

			var move = GetMoveFromType(result.Type);

			var newNode = new InstructionNode(move, result, operand1)
			{
				Label = label
			};

			node.Previous.Insert(newNode);
		}

		protected abstract bool IsThreeTwoAddressRequired(BaseInstruction instruction);

		protected abstract BaseInstruction GetMoveFromType(MosaType type);
	}
}
