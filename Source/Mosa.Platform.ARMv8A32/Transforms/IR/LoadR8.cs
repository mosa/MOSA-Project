// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadR8
/// </summary>
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(IRInstruction.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARMv8A32.Ldf, context.Result, context.Operand1, context.Operand2);
	}
}
