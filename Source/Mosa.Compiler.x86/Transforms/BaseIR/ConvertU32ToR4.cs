// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToU32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class ConvertU32ToR4 : BaseIRTransform
{
	public ConvertU32ToR4() : base(Framework.IR.ConvertU32ToR4, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		//operand1 = MoveConstantToFloatRegister(transform, context, operand1);

		//context.SetInstruction(X86.Xorps, result, result, result);
		//context.SetInstruction(X86.Movss, result, operand1);
		context.SetInstruction(X86.Cvtsi2ss32, result, operand1);
	}
}
