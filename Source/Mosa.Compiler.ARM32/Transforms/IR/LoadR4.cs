// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// LoadR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class LoadR4 : BaseIRTransform
{
	public LoadR4() : base(Framework.IR.LoadR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.Ldf, context.Result, context.Operand1, context.Operand2);
	}
}
