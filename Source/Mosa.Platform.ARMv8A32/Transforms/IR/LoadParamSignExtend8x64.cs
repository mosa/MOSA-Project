// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParamSignExtend8x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadParamSignExtend8x64 : BaseIRTransform
{
	public LoadParamSignExtend8x64() : base(IRInstruction.LoadParamSignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);

		TransformLoad(transform, context, ARMv8A32.LdrS8, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, Operand.Constant32_31);
	}
}
