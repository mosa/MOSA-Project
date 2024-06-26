// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// StoreParamR4
/// </summary>
public sealed class StoreParamR4 : BaseIRTransform
{
	public StoreParamR4() : base(IR.StoreParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//operand1 = MoveConstantToRegister(transform, context, operand1);

		TransformFloatingPointStore(transform, context, ARM32.VStr, transform.StackFrame, context.Operand1, context.Operand2);
	}
}
