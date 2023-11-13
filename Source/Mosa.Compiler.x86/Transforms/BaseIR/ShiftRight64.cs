// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ShiftRight64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class ShiftRight64 : BaseIRTransform
{
	public ShiftRight64() : base(IR.ShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		/// Optimized shift when shift value is a constant and 32 or more, or zero
		if (count.IsResolvedConstant)
		{
			var shift = count.ConstantUnsigned32 & 0b111111;

			if (shift == 0)
			{
				// shift is exactly 0 bits (nop)
				context.SetInstruction(X86.Mov32, resultLow, op1L);
				context.AppendInstruction(X86.Mov32, resultHigh, op1H);
				return;
			}
			else if (shift == 32)
			{
				// shift is exactly 32 bits
				context.SetInstruction(X86.Mov32, resultLow, op1H);
				context.AppendInstruction(X86.Mov32, resultHigh, Operand.Constant32_0);
				return;
			}
			else if (shift > 32)
			{
				// shift is greater than 32 bits
				var newshift = Operand.CreateConstant32(shift - 32);
				context.SetInstruction(X86.Shr32, resultLow, op1H, newshift);
				context.AppendInstruction(X86.Mov32, resultHigh, Operand.Constant32_0);
				return;
			}
		}

		var newBlocks = transform.CreateNewBlockContexts(1, context.Label);
		var nextBlock = transform.Split(context);

		var ECX = transform.PhysicalRegisters.Allocate32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ECX, count);
		context.AppendInstruction(X86.Shrd32, resultLow, op1L, op1H, ECX);
		context.AppendInstruction(X86.Shr32, resultHigh, op1H, ECX);

		context.AppendInstruction(X86.Test32, null, ECX, Operand.Constant32_32);
		context.AppendInstruction(X86.Branch, ConditionCode.Equal, nextBlock.Block);
		context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

		newBlocks[0].AppendInstruction(X86.Mov32, resultLow, resultHigh);
		newBlocks[0].AppendInstruction(X86.Mov32, resultHigh, Operand.Constant32_0);
		newBlocks[0].AppendInstruction(X86.Jmp, nextBlock.Block);
	}
}
