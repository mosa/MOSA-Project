// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.Intel.Stages
{
	/// <summary>
	/// Floating Point Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformTransformationStage" />
	public abstract class FloatingPointStage : BasePlatformTransformationStage
	{
		#region Abstract Methods

		protected abstract bool IsLoad(BaseInstruction instruction);

		protected abstract bool IsIntegerToFloating(BaseInstruction instruction);

		protected abstract BaseInstruction MovssLoad { get; }

		protected abstract BaseInstruction MovsdLoad { get; }

		#endregion Abstract Methods

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
					if (node.IsEmpty || !(node.Instruction is BasePlatformInstruction))
						continue;

					if (node.OperandCount == 0)
						continue;

					if (IsIntegerToFloating(node.Instruction) && node.Operand1.IsConstant)
					{
						FixConstantIntegerToFloat(node);
						continue;
					}

					if (IsLoad(node.Instruction))
						continue;

					EmitFloatingPointConstants(node);
				}
			}
		}

		protected void FixConstantIntegerToFloat(InstructionNode node)
		{
			var source = node.Operand1;
			var result = node.Result;

			if (result.IsR4)
			{
				var symbol = Linker.GetConstantSymbol((float)source.ConstantUnsignedLongInteger);
				var label = Operand.CreateLabel(result.Type, symbol.Name);
				node.SetInstruction(MovssLoad, result, label, ConstantZero);
			}
			else if (result.IsR8)
			{
				var symbol = Linker.GetConstantSymbol((double)source.ConstantUnsignedLongInteger);
				var label = Operand.CreateLabel(result.Type, symbol.Name);
				node.SetInstruction(MovsdLoad, result, label, ConstantZero);
			}
		}

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="node">The node.</param>
		protected void EmitFloatingPointConstants(InstructionNode node)
		{
			for (int i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (operand == null || !operand.IsConstant || !operand.IsR)
					continue;

				if (operand.IsUnresolvedConstant)
					continue;

				var before = new Context(node).InsertBefore();

				var v1 = AllocateVirtualRegister(operand.Type);

				if (operand.IsR4)
				{
					var symbol = Linker.GetConstantSymbol(operand.ConstantSingleFloatingPoint);
					var label = Operand.CreateLabel(operand.Type, symbol.Name);
					before.SetInstruction(MovssLoad, v1, label, ConstantZero);
				}
				else
				{
					var symbol = Linker.GetConstantSymbol(operand.ConstantDoubleFloatingPoint);
					var label = Operand.CreateLabel(operand.Type, symbol.Name);
					before.SetInstruction(MovsdLoad, v1, label, ConstantZero);
				}

				node.SetOperand(i, v1);
			}
		}
	}
}
