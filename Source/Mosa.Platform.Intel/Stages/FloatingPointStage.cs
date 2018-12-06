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

					// fixme - must be a better way
					if (IsLoad(node.Instruction))
						continue;

					EmitFloatingPointConstants(node);
				}
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

				var v1 = AllocateVirtualRegister(operand.Type);

				var symbol = (operand.IsR4) ?
					MethodCompiler.Linker.GetConstantSymbol(operand.ConstantSingleFloatingPoint)
					: MethodCompiler.Linker.GetConstantSymbol(operand.ConstantDoubleFloatingPoint);

				var s1 = Operand.CreateLabel(operand.Type, symbol.Name);

				var before = new Context(node).InsertBefore();

				if (operand.IsR4)
				{
					before.SetInstruction(MovssLoad, v1, s1, ConstantZero);
				}
				else
				{
					before.SetInstruction(MovsdLoad, v1, s1, ConstantZero);
				}

				node.SetOperand(i, v1);
			}
		}
	}
}
