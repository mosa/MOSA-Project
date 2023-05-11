// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// StoreParam64
/// </summary>
[Transform("x86.IR")]
public sealed class StoreParam64 : BaseIRTransform
{
	public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Operand1, out var op0L, out var op0H);
		transform.SplitOperand(context.Operand2, out var op1L, out var op1H);

		context.SetInstruction(X86.MovStore32, null, transform.StackFrame, op0L, op1L);
		context.AppendInstruction(X86.MovStore32, null, transform.StackFrame, op0H, op1H);
	}
}
