// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// ConvertU32ToR8
/// </summary>
[Transform("x86.IR")]
public sealed class ConvertU32ToR8 : BaseIRTransform
{
	public ConvertU32ToR8() : base(IRInstruction.ConvertU32ToR8, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		//operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		//context.SetInstruction(X86.Xorps, result, result, result);
		//context.SetInstruction(X86.Movss, result, operand1);
		context.SetInstruction(X86.Cvtsi2sd32, result, operand1);
	}
}
