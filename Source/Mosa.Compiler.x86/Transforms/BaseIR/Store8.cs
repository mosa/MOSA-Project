// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Store8
/// </summary>
[Transform]
public sealed class Store8 : BaseIRTransform
{
	public Store8() : base(IR.Store8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovStore8, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
