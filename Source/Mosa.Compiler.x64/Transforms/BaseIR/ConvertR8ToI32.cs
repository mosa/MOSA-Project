// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToI32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertR8ToI32 : BaseIRTransform
{
	public ConvertR8ToI32() : base(Framework.IR.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		context.SetInstruction(X64.Cvttsd2si32, result, operand1);
	}
}
