// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// SignExtend16x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class SignExtend16x64 : BaseIRTransform
{
	public SignExtend16x64() : base(IRInstruction.SignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

		var op1 = MoveConstantToRegister(transform, context, context.Operand1);

		context.SetInstruction(ARMv8A32.Lsl, resultLow, op1, transform.Constant32_16);
		context.AppendInstruction(ARMv8A32.Asr, resultLow, resultLow, transform.Constant32_16);
		context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, transform.Constant32_31);
	}
}
