// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Store8
/// </summary>
[Transform("x64.IR")]
public sealed class Store8 : BaseIRTransform
{
	public Store8() : base(IRInstruction.Store8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore8, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
