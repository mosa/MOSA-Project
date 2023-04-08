// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// Xor64
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class Xor64 : BaseIRTransform
{
	public Xor64() : base(IRInstruction.Xor64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);
		op2L = MoveConstantToRegisterOrImmediate(transform, context, op2L);
		op2H = MoveConstantToRegisterOrImmediate(transform, context, op2H);

		context.SetInstruction(ARMv8A32.Eor, resultLow, op1L, op2L);
		context.AppendInstruction(ARMv8A32.Eor, resultHigh, op1H, op2H);
	}
}
