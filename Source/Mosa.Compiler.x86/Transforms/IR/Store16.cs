// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Store16
/// </summary>
[Transform("x86.IR")]
public sealed class Store16 : BaseIRTransform
{
	public Store16() : base(Framework.IR.Store16, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovStore16, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
