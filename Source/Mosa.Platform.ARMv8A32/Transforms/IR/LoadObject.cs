// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadObject
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadObject : BaseIRTransform
{
	public LoadObject() : base(IRInstruction.LoadObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARMv8A32.Ldr32, context.Result, transform.StackFrame, context.Operand1);
	}
}
