// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Compare64x32
/// </summary>
public sealed class Compare64x32 : BaseIRTransform
{
	public Compare64x32() : base(IR.Compare64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var condition = context.ConditionCode;
		var resultOperand = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		context.SetInstruction(X64.Cmp64, null, operand1, operand2);
		context.AppendInstruction(X64.Setcc, condition, v1);
		context.AppendInstruction(X64.Movzx8To64, resultOperand, v1);
	}
}
