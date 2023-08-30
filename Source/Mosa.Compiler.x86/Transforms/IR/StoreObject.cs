// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// StoreObject
/// </summary>
[Transform("x86.IR")]
public sealed class StoreObject : BaseIRTransform
{
	public StoreObject() : base(IRInstruction.StoreObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
