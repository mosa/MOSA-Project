// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadSignExtend8x32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadSignExtend8x32 : BaseIRTransform
{
	public LoadSignExtend8x32() : base(IR.LoadSignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.LdrS8, context.Result, transform.StackFrame, context.Operand1);
	}
}
