// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// StoreR8
/// </summary>
[Transform("x86.IR")]
public sealed class StoreR8 : BaseIRTransform
{
	public StoreR8() : base(IRInstruction.StoreR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		operand3 = MoveConstantToFloatRegister(transform, context, operand3);

		context.SetInstruction(X86.MovsdStore, null, operand1, operand2, operand3);
	}
}
