// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// LoadParamZeroExtend32x64
/// </summary>
[Transform("x86.IR")]
public sealed class LoadParamZeroExtend32x64 : BaseIRTransform
{
	public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out _);

		context.SetInstruction(X86.MovLoad32, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(X86.Mov32, resultHigh, Operand.Constant32_0);
	}
}
