// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadManagedPointer
/// </summary>
[Transform]
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

		context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
	}
}
