// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// MoveR8
/// </summary>
[Transform("x86.BaseIR")]
public sealed class MoveR8 : BaseIRTransform
{
	public MoveR8() : base(Framework.IR.MoveR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Movsd, result, operand1);
	}
}
