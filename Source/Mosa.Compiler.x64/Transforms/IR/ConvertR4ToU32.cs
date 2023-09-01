// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ConvertR4ToU32
/// </summary>
[Transform("x64.IR")]
public sealed class ConvertR4ToU32 : BaseIRTransform
{
	public ConvertR4ToU32() : base(IRInstruction.ConvertR4ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Cvttss2si64, result, operand1);
	}
}
