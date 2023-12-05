// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Neg64
/// </summary>
public sealed class Neg64 : BaseIRTransform
{
	public Neg64() : base(IR.Neg64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(ARM32.Mov, v1, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Sub, InstructionOption.SetFlags, resultLow, v1, op1L);
		context.AppendInstruction(ARM32.Sbc, resultHigh, v1, op1H);
	}
}
