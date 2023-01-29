// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Truncate64x32
/// </summary>
public sealed class Truncate64x32 : BaseTransform
{
	public Truncate64x32() : base(IRInstruction.Truncate64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Operand1.IsInteger64);
		Debug.Assert(!context.Result.IsInteger64);
		transform.SplitLongOperand(context.Operand1, out var op1L, out _);

		context.SetInstruction(X86.Mov32, context.Result, op1L);
	}
}
