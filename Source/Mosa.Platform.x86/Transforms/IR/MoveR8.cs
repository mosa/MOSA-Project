// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// MoveR8
/// </summary>
public sealed class MoveR8 : BaseTransform
{
	public MoveR8() : base(IRInstruction.MoveR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Movsd, result, operand1);
	}
}
