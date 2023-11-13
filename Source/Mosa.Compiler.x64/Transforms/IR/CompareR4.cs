// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// CompareR4
/// </summary>
[Transform("x64.IR")]
public sealed class CompareR4 : BaseIRTransform
{
	public CompareR4() : base(Framework.IR.CompareR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//FloatCompare(context, X64.Ucomiss);

		var instruction = X64.Ucomiss;

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
			context.AppendInstruction(X64.Setcc, ConditionCode.NoParity, result);
			context.AppendInstruction(X64.Mov32, v1, Operand.Constant32_0);
			context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, v1);
			context.AppendInstruction(X64.Movzx8To32, result, result);
			return;
		}
		else if (condition == ConditionCode.NotEqual)
		{
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, ConditionCode.Parity, result);
			context.AppendInstruction(X64.Mov32, v1, Operand.CreateConstant32(1));
			context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, v1);
			context.AppendInstruction(X64.Movzx8To32, result, result);
			return;
		}
		else if (condition is ConditionCode.Greater or ConditionCode.UnsignedGreater)
		{
			context.SetInstruction(instruction, null, operand1, operand2);
			context.AppendInstruction(X64.Setcc, ConditionCode.UnsignedGreater, v1);
			context.AppendInstruction(X64.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.Less or ConditionCode.UnsignedLess)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X64.Setcc, ConditionCode.UnsignedGreater, v1);
			context.AppendInstruction(X64.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.GreaterOrEqual or ConditionCode.UnsignedGreaterOrEqual)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X64.Setcc, ConditionCode.NoCarry, v1);
			context.AppendInstruction(X64.Movzx8To32, result, v1);
			return;
		}
		else if (condition is ConditionCode.LessOrEqual or ConditionCode.UnsignedLessOrEqual)
		{
			context.SetInstruction(instruction, null, operand2, operand1);
			context.AppendInstruction(X64.Setcc, ConditionCode.NoCarry, v1);
			context.AppendInstruction(X64.Movzx8To32, result, v1);
			return;
		}
	}
}
