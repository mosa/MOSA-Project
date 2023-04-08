// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadR4
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadR4 : BaseIRTransform
{
	public LoadR4() : base(IRInstruction.LoadR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARMv8A32.Ldf, context.Result, context.Operand1, context.Operand2);
	}
}
