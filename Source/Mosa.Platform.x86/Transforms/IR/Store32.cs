// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Store32
/// </summary>
public sealed class Store32 : BaseIRTransform
{
	public Store32() : base(IRInstruction.Store32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
