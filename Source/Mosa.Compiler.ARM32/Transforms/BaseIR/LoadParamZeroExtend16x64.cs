// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadParamZeroExtend16x64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadParamZeroExtend16x64 : BaseIRTransform
{
	public LoadParamZeroExtend16x64() : base(IR.LoadParamZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out _);

		TransformLoad(transform, context, ARM32.Ldr16, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(ARM32.Mov, resultHigh, Operand.Constant32_0);
	}
}
