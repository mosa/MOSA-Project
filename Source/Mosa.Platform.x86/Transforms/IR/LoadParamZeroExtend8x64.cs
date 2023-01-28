// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend8x64
/// </summary>
public sealed class LoadParamZeroExtend8x64 : BaseTransform
{
	public LoadParamZeroExtend8x64() : base(IRInstruction.LoadParamZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var lowOffset, out _);

		context.SetInstruction(X86.MovLoad8, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(X86.Mov32, resultHigh, transform.Constant32_0);
	}
}
