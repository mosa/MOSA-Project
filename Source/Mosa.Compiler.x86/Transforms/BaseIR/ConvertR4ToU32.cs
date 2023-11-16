// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertR4ToU32
/// </summary>
[Transform]
public sealed class ConvertR4ToU32 : BaseIRTransform
{
	public ConvertR4ToU32() : base(IR.ConvertR4ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Cvttss2si32, result, operand1);
	}
}
