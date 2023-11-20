// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// SubCarryOut32
/// </summary>
public sealed class SubCarryOut32 : BaseIRTransform
{
	public SubCarryOut32() : base(IR.SubCarryOut32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X64.Sub32, result, operand1, operand2);
		context.AppendInstruction(X64.Setcc, ConditionCode.Carry, v1);
		context.AppendInstruction(X64.Movzx8To32, result2, v1);
	}
}
