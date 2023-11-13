// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// StoreParamR4
/// </summary>
[Transform("x64.IR")]
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(Framework.IR.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		operand2 = MoveConstantToFloatRegister(transform, context, operand2);

		context.SetInstruction(X64.MovssStore, null, transform.StackFrame, operand1, operand2);
	}
}
