// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// CompareR8
/// </summary>
[Transform("x86.IR")]
public sealed class CompareR8 : BaseIRTransform
{
	public CompareR8() : base(IRInstruction.CompareR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
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
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.NoParity, result);
			context.AppendInstruction(X86.Mov32, v1, transform.Constant32_0);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
			context.AppendInstruction(X86.Movzx8To32, result, result);
			return;
		}
		else if (condition == ConditionCode.NotEqual)
		{
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X86.Setcc, ConditionCode.Parity, result);
			context.AppendInstruction(X86.Mov32, v1, transform.Constant32_1);
			context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
			context.AppendInstruction(X86.Movzx8To32, result, result);
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
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.LessOrEqual or ConditionCode.UnsignedLessOrEqual)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
			context.AppendInstruction(X86.Movzx8To32, result, v1);
			return;
		}
	}
}
