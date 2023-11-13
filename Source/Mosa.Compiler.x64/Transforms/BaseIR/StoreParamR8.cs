// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreParamR8
/// </summary>
[Transform("x64.BaseIR")]
public sealed class StoreParamR8 : BaseIRTransform
{
	public StoreParamR8() : base(Framework.IR.StoreParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X64.MovsdStore, null, transform.StackFrame, operand1, operand2);
	}
}
