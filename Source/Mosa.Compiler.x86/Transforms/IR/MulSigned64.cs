// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// MulSigned64
/// </summary>
[Transform("x86.IR")]
public sealed class MulSigned64 : BaseIRTransform
{
	public MulSigned64() : base(IRInstruction.MulSigned64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction2(X86.Mul32, v1, resultLow, op2L, op1L);

		if (!resultHigh.IsConstantZero)
		{
			context.AppendInstruction(X86.Mov32, v2, op1L);
			context.AppendInstruction(X86.IMul32, v3, v2, op2H);
			context.AppendInstruction(X86.Add32, v4, v1, v3);
			context.AppendInstruction(X86.Mov32, v3, op2L);
			context.AppendInstruction(X86.IMul32, v3, v3, op1H);
			context.AppendInstruction(X86.Add32, resultHigh, v4, v3);
		}
	}
}
