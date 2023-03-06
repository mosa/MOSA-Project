// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// StoreObject
/// </summary>
public sealed class StoreObject : BaseIRTransform
{
	public StoreObject() : base(IRInstruction.StoreObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore64, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
