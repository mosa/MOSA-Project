// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Load32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Load32 : BaseIRTransform
{
	public Load32() : base(IR.Load32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.Ldr32, context.Result, context.Operand1, context.Operand2);
	}
}
