// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// Load64
/// </summary>
[Transform("x64.IR")]
public sealed class Load64 : BaseIRTransform
{
	public Load64() : base(IRInstruction.Load64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovLoad64, context.Result, context.Operand1, context.Operand2);
	}
}
