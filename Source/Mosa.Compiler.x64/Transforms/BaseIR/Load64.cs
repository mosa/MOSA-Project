// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Load64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class Load64 : BaseIRTransform
{
	public Load64() : base(Framework.IR.Load64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X64.MovLoad64, context.Result, context.Operand1, context.Operand2);
	}
}
