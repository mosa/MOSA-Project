// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend8x64
/// </summary>
[Transform("x86.IR")]
public sealed class LoadParamZeroExtend8x64 : BaseIRTransform
{
	public LoadParamZeroExtend8x64() : base(IRInstruction.LoadParamZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var lowOffset, out _);

		context.SetInstruction(X86.MovLoad8, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(X86.Mov32, resultHigh, transform.Constant32_0);
	}
}
