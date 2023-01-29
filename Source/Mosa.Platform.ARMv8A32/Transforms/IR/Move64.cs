// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Move64
/// </summary>
public sealed class Move64 : BaseTransform
{
	public Move64() : base(IRInstruction.Move64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

		op1L = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1L);
		op1H = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1H);

		context.SetInstruction(ARMv8A32.Mov, resultLow, op1L);
		context.AppendInstruction(ARMv8A32.Mov, resultHigh, op1H);
	}
}
