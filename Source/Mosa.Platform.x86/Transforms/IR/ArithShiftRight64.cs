// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ArithShiftRight64
/// </summary>
public sealed class ArithShiftRight64 : BaseTransform
{
	public ArithShiftRight64() : base(IRInstruction.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		var v1_count = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();
		var v3 = transform.AllocateVirtualRegister32();

		var newBlocks = transform.CreateNewBlockContexts(4, context.Label);
		var nextBlock = transform.Split(context);

		context.SetInstruction(X86.Mov32, v1_count, count);
		context.AppendInstruction(X86.Cmp32, null, v1_count, transform.Constant32_64);
		context.AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[3].Block);
		context.AppendInstruction(X86.Jmp, newBlocks[0].Block);

		newBlocks[0].AppendInstruction(X86.Cmp32, null, v1_count, transform.Constant32_32);
		newBlocks[0].AppendInstruction(X86.Branch, ConditionCode.GreaterOrEqual, newBlocks[2].Block);
		newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[1].Block);

		newBlocks[1].AppendInstruction(X86.Shrd32, resultLow, op1L, op1H, v1_count);
		newBlocks[1].AppendInstruction(X86.Sar32, resultHigh, op1H, v1_count);
		newBlocks[1].AppendInstruction(X86.Jmp, nextBlock.Block);

		newBlocks[2].AppendInstruction(X86.Mov32, v2, op1H);
		newBlocks[2].AppendInstruction(X86.Sar32, resultHigh, op1H, transform.Constant32_31);
		newBlocks[2].AppendInstruction(X86.And32, v3, v1_count, transform.Constant32_31);
		newBlocks[2].AppendInstruction(X86.Sar32, resultLow, v2, v3);
		newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

		newBlocks[3].AppendInstruction(X86.Sar32, resultHigh, op1H, transform.Constant32_31);
		newBlocks[3].AppendInstruction(X86.Mov32, resultLow, resultHigh);
		newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
	}
}
