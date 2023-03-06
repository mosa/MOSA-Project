// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// MulR4
/// </summary>
public sealed class MulR4 : BaseIRTransform
{
	public MulR4() : base(IRInstruction.MulR4, TransformType.Manual | TransformType.Transform)
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
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X86.Mulss, result, operand1, operand2);
	}
}
