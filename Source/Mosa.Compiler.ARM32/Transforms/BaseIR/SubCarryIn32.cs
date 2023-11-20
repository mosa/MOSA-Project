// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// SubCarryIn32
/// </summary>
public sealed class SubCarryIn32 : BaseIRTransform
{
	public SubCarryIn32() : base(IR.SubCarryIn32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		operand1 = MoveConstantToRegister(transform, context, operand1);
		operand2 = MoveConstantToRegisterOrImmediate(transform, context, operand2);
		operand3 = MoveConstantToRegisterOrImmediate(transform, context, operand3);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(ARM32.Sub, v1, operand1, operand2);
		context.AppendInstruction(ARM32.Sub, result, v1, operand3);
	}
}
