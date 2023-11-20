// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Truncate64x32
/// </summary>
public sealed class Truncate64x32 : BaseIRTransform
{
	public Truncate64x32() : base(IR.Truncate64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsInt64);
		Debug.Assert(!context.Result.IsInt64);
		transform.SplitOperand(context.Operand1, out var op1L, out _);

		context.SetInstruction(X86.Mov32, context.Result, op1L);
	}
}
