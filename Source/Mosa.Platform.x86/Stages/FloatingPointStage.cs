// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;

// fixme: this stage may not be necessary with the specific load/store instructions

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public class FloatingPointStage : BaseTransformationStage
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
					if (node.IsEmpty || !(node.Instruction is X86Instruction))
						continue;

					if (node.OperandCount == 0)
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

				var v1 = AllocateVirtualRegister(operand.Type);

				var symbol = (operand.IsR4) ?
					MethodCompiler.Linker.GetConstantSymbol(operand.ConstantSingleFloatingPoint)
					: MethodCompiler.Linker.GetConstantSymbol(operand.ConstantDoubleFloatingPoint);

				var s1 = Operand.CreateLabel(operand.Type, symbol.Name);

				var context = new Context(node);

				var loadInstruction = GetLoadInstruction(operand.Type);

				var size = (operand.IsR4) ? InstructionSize.Size32 : InstructionSize.Size64;

				context.InsertBefore().SetInstruction(loadInstruction, size, v1, s1, ConstantZero);

				node.SetOperand(i, v1);
			}
		}
	}
}
