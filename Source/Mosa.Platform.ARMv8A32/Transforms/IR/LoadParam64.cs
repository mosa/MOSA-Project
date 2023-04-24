// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParam64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadParam64 : BaseIRTransform
{
	public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);

		TransformLoad(transform, context, ARMv8A32.Ldr32, resultLow, transform.StackFrame, lowOffset);
		TransformLoad(transform, context.InsertAfter(), ARMv8A32.Ldr32, resultHigh, transform.StackFrame, highOffset);
	}
}
