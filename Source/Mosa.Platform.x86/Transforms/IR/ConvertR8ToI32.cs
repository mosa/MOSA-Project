// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// ConvertR8ToI32
/// </summary>
[Transform("x86.IR")]
public sealed class ConvertR8ToI32 : BaseIRTransform
{
	public ConvertR8ToI32() : base(IRInstruction.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X86.Cvttsd2si32, result, operand1);
	}
}
