// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// MoveR4
/// </summary>
[Transform("x64.IR")]
public sealed class MoveR4 : BaseIRTransform
{
	public MoveR4() : base(IRInstruction.MoveR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Movss, result, operand1);
	}
}
