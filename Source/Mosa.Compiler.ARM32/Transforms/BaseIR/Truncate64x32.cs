// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Truncate64x32
/// </summary>
[Transform]
public sealed class Truncate64x32 : BaseIRTransform
{
	public Truncate64x32() : base(IR.Truncate64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsInt64);
		Debug.Assert(!context.Result.IsInt64);

		transform.SplitOperand(context.Result, out var resultLow, out _);
		transform.SplitOperand(context.Operand1, out var op1L, out _);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);

		context.SetInstruction(ARM32.Mov, StatusRegister.Set, resultLow, op1L);
	}
}
