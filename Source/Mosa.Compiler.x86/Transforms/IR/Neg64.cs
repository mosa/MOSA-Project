// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Neg64
/// </summary>
[Transform("x86.IR")]
public sealed class Neg64 : BaseIRTransform
{
	public Neg64() : base(Framework.IR.Neg64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Neg32, resultLow, op1L);
		context.AppendInstruction(X86.Adc32, v1, op1H, Operand.Constant32_0);
		context.AppendInstruction(X86.Neg32, resultHigh, v1);
	}
}
