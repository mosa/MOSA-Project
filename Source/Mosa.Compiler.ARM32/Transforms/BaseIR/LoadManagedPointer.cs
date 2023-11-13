// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadManagedPointer
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadManagedPointer : BaseIRTransform
{
	public LoadManagedPointer() : base(IR.LoadManagedPointer, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		transform.OrderLoadStoreOperands(context);

		TransformLoad(transform, context, ARM32.Ldr32, context.Result, transform.StackFrame, context.Operand1);
	}
}
