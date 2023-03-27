// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// StoreR4
/// </summary>
[Transform("x86.IR")]
public sealed class StoreR4 : BaseIRTransform
{
	public StoreR4() : base(IRInstruction.StoreR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		operand3 = MoveConstantToFloatRegister(transform, context, operand3);

		context.SetInstruction(X86.MovssStore, null, operand1, operand2, operand3);
	}
}
