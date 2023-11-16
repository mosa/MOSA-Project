// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadSignExtend16x32
/// </summary>
[Transform]
public sealed class LoadSignExtend16x32 : BaseIRTransform
{
	public LoadSignExtend16x32() : base(IR.LoadSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.LdrS16, context.Result, transform.StackFrame, context.Operand1);
	}
}
