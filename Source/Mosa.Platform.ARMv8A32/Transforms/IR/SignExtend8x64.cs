// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// SignExtend8x64
/// </summary>
public sealed class SignExtend8x64 : BaseTransform
{
	public SignExtend8x64() : base(IRInstruction.SignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

		var op1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, context.Operand1);

		context.SetInstruction(ARMv8A32.Lsl, resultLow, op1, transform.Constant32_24);
		context.AppendInstruction(ARMv8A32.Asr, resultLow, resultLow, transform.Constant32_24);
		context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, transform.Constant32_31);
	}
}
