// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// StoreObject
/// </summary>
[Transform("x64.BaseIR")]
public sealed class StoreObject : BaseIRTransform
{
	public StoreObject() : base(Framework.IR.StoreObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore64, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
