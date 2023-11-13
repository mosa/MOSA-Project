// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadR8
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(Framework.IR.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.Ldf, context.Result, context.Operand1, context.Operand2);
	}
}
