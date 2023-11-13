// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToU32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertU32ToR4 : BaseIRTransform
{
	public ConvertU32ToR4() : base(IR.ConvertU32ToR4, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		//operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		//context.SetInstruction(X64.Xorps, result, result, result);
		//context.SetInstruction(X64.Movss, result, operand1);
		context.SetInstruction(X64.Cvtsi2ss32, result, operand1);
	}
}
