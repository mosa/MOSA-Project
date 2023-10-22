// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// DivR8
/// </summary>
[Transform("x86.IR")]
public sealed class DivR8 : BaseIRTransform
{
	public DivR8() : base(IRInstruction.DivR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand1 = MoveConstantToFloatRegister(transform, context, operand1);
		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X86.Divsd, result, operand1, operand2);
	}
}
