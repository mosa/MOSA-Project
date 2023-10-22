// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadObject
/// </summary>
[Transform("x64.IR")]
public sealed class LoadObject : BaseIRTransform
{
	public LoadObject() : base(IRInstruction.LoadObject, TransformType.Manual | TransformType.Transform)
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
