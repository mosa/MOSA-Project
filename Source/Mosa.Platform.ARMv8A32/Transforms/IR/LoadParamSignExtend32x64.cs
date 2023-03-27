// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParamSignExtend32x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadParamSignExtend32x64 : BaseIRTransform
{
	public LoadParamSignExtend32x64() : base(IRInstruction.LoadParamSignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var lowOffset, out var highOffset);

		TransformLoad(transform, context, ARMv8A32.Ldr32, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(ARMv8A32.Asr, resultHigh, resultLow, transform.Constant32_31);
	}
}
