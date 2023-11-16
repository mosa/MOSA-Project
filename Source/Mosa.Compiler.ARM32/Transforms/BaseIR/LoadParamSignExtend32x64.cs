// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadParamSignExtend32x64
/// </summary>
[Transform]
public sealed class LoadParamSignExtend32x64 : BaseIRTransform
{
	public LoadParamSignExtend32x64() : base(IR.LoadParamSignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);

		TransformLoad(transform, context, ARM32.Ldr32, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(ARM32.Asr, resultHigh, resultLow, Operand.Constant32_31);
	}
}
