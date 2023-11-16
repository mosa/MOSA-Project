// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// StoreParamR4
/// </summary>
[Transform]
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(IR.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X86.MovssStore, null, transform.StackFrame, operand1, operand2);
	}
}
