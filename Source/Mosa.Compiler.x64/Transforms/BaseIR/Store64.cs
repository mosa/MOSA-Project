// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Store64
/// </summary>
[Transform]
public sealed class Store64 : BaseIRTransform
{
	public Store64() : base(IR.Store64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore64, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
