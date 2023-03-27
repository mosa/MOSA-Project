// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend16x64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadParamZeroExtend16x64 : BaseIRTransform
{
	public LoadParamZeroExtend16x64() : base(IRInstruction.LoadParamZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var lowOffset, out _);

		TransformLoad(transform, context, ARMv8A32.Ldr16, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(ARMv8A32.Mov, resultHigh, transform.Constant32_0);
	}
}
