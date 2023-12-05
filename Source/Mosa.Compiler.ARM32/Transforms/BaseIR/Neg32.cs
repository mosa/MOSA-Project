// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Neg32
/// </summary>
public sealed class Neg32 : BaseIRTransform
{
	public Neg32() : base(IR.Neg32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(ARM32.Mov, v1, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Sub, result, v1, operand1);
	}
}
