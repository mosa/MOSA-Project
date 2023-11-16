// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// AddR4
/// </summary>
[Transform]
public sealed class AddR4 : BaseIRTransform
{
	public AddR4() : base(IR.AddR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X64.Addss, result, operand1, operand2);
	}
}