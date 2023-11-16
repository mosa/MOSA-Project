// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToR4
/// </summary>
[Transform]
public sealed class ConvertR8ToR4 : BaseIRTransform
{
	public ConvertR8ToR4() : base(IR.ConvertR8ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Cvtsd2ss, result, operand1);
	}
}
