// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadR8
/// </summary>
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(IR.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.OrderLoadStoreOperands(context);

		TransformFloatingPointLoad(transform, context, ARM32.VLdr, context.Result, context.Operand1, context.Operand2);
	}
}
