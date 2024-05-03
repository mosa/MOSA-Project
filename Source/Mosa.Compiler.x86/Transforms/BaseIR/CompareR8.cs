// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// CompareR8
/// </summary>
public sealed class CompareR8 : BaseIRTransform
{
	public CompareR8() : base(IR.CompareR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.ConditionCode != ConditionCode.Undefined);

		//FloatCompare(context, X86.Ucomisd);

		var instruction = X86.Ucomisd;

		var result = context.Result;
		var condition = context.ConditionCode;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		var v1 = transform.VirtualRegisters.Allocate32();

		if (condition == ConditionCode.Equal)
		{
			var nextBlock = transform.Split(context);
			var newBlocks = transform.CreateNewBlockContexts(2, context.Label);

			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.NoParity, result);
			context.AppendInstruction(X86.Branch, ConditionCode.Parity, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Setcc, ConditionCode.Equal, result);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Movzx8To32, result, result);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);

			return;
		}
		else if (condition == ConditionCode.NotEqual)
		{
			var nextBlock = transform.Split(context);
			var newBlocks = transform.CreateNewBlockContexts(2, context.Label);

			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.Parity, result);
			context.AppendInstruction(X86.Branch, ConditionCode.Parity, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(X86.Setcc, ConditionCode.NotEqual, result);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

			newBlocks[1].AppendInstruction(X86.Movzx8To32, result, result);
			newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);

			return;
		}
		else if (condition is ConditionCode.Greater or ConditionCode.UnsignedGreater)
		{
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.Less or ConditionCode.UnsignedLess)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.GreaterOrEqual or ConditionCode.UnsignedGreaterOrEqual)
		{
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.LessOrEqual or ConditionCode.UnsignedLessOrEqual)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreaterOrEqual, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
	}
}
