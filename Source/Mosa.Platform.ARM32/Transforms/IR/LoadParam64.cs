// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// LoadParam64
/// </summary>
[Transform("ARM32.IR")]
public sealed class LoadParam64 : BaseIRTransform
{
	public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);

		TransformLoad(transform, context, ARM32.Ldr32, resultLow, transform.StackFrame, lowOffset);
		TransformLoad(transform, context.InsertAfter(), ARM32.Ldr32, resultHigh, transform.StackFrame, highOffset);
	}
}
