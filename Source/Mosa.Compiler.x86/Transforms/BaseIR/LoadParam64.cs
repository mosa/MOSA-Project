// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParam64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadParam64 : BaseIRTransform
{
	public LoadParam64() : base(Framework.IR.LoadParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var lowOffset, out var highOffset);

		context.SetInstruction(X86.MovLoad32, resultLow, transform.StackFrame, lowOffset);
		context.AppendInstruction(X86.MovLoad32, resultHigh, transform.StackFrame, highOffset);
	}
}
