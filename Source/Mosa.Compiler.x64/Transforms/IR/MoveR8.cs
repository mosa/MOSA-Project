// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// MoveR8
/// </summary>
[Transform("x64.IR")]
public sealed class MoveR8 : BaseIRTransform
{
	public MoveR8() : base(IRInstruction.MoveR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Movsd, result, operand1);
	}
}
