// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Add64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class Add64 : BaseIRTransform
{
	public Add64() : base(Framework.IR.Add64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		context.SetInstruction(X86.Add32, resultLow, op1L, op2L);
		context.AppendInstruction(X86.Adc32, resultHigh, op1H, op2H);
	}
}
