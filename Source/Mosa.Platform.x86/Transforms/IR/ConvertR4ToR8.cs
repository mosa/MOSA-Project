// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ConvertR4ToR8
/// </summary>
public sealed class ConvertR4ToR8 : BaseTransform
{
	public ConvertR4ToR8() : base(IRInstruction.ConvertR4ToR8, TransformType.Manual | TransformType.Transform)
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

		context.SetInstruction(X86.Cvtss2sd, result, operand1);
	}
}
