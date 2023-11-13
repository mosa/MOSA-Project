// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// StoreManagedPointer
/// </summary>
[Transform("x86.BaseIR")]
public sealed class StoreManagedPointer : BaseIRTransform
{
	public StoreManagedPointer() : base(Framework.IR.StoreManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovStore32, null, context.Operand1, context.Operand2, context.Operand3);
	}
}
