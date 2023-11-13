// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Store32
/// </summary>
[Transform("x64.IR")]
public sealed class Store32 : BaseIRTransform
{
	public Store32() : base(Framework.IR.Store32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
