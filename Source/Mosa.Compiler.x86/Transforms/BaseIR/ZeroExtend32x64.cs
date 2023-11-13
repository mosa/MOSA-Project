// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ZeroExtend32x64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class ZeroExtend32x64 : BaseIRTransform
{
	public ZeroExtend32x64() : base(Framework.IR.ZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out _);

		context.SetInstruction(X86.Mov32, resultLow, op1L);
		context.AppendInstruction(X86.Mov32, resultHigh, Operand.Constant32_0);
	}
}
