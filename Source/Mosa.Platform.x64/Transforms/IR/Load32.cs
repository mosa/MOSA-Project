// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Load32
/// </summary>
public sealed class Load32 : BaseIRTransform
{
	public Load32() : base(IRInstruction.Load32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovLoad32, context.Result, context.Operand1, context.Operand2);
	}
}
