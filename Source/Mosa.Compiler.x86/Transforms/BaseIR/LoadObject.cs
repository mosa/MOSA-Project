// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadObject
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadObject : BaseIRTransform
{
	public LoadObject() : base(IR.LoadObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
	}
}
