// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// ConvertR4ToR8
/// </summary>
[Transform("x64.IR")]
public sealed class ConvertR4ToR8 : BaseIRTransform
{
	public ConvertR4ToR8() : base(IRInstruction.ConvertR4ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Cvtss2sd, result, operand1);
	}
}
