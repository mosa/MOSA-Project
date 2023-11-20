// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToI32
/// </summary>
public sealed class ConvertR8ToI32 : BaseIRTransform
{
	public ConvertR8ToI32() : base(IR.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Cvttsd2si32, result, operand1);
	}
}
