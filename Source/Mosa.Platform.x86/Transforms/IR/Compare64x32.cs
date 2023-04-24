// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Compare64x32
/// </summary>
[Transform("x86.IR")]
public sealed class Compare64x32 : BaseIRTransform
{
	public Compare64x32() : base(IRInstruction.Compare64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		//Compare64x64(context);

		Debug.Assert(context.Operand1 != null && context.Operand2 != null);
		Debug.Assert(context.Result.IsVirtualRegister);

		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		var result = context.Result;
		var condition = context.ConditionCode;

		var nextBlock = transform.Split(context);
		var newBlocks = transform.CreateNewBlockContexts(4, context.Label);

		if (op1H.IsResolvedConstant && op2H.IsResolvedConstant)
		{
			// high dwords are constants
			context.SetInstruction(X86.Jmp, op1H.ConstantUnsigned32 == op2H.ConstantUnsigned32 ? newBlocks[1].Block : newBlocks[0].Block);
		}
		else
		{
			// Compare high dwords
			context.SetInstruction(X86.Cmp32, null, op1H, op2H);
			context.AppendInstruction(X86.Branch, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(X86.Jmp, newBlocks[0].Block);
		}

		// Branch if check already gave results
		if (condition == ConditionCode.Equal)
		{
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
		}
		else
		{
			// Branch if check already gave results
			newBlocks[0].AppendInstruction(X86.Branch, condition, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(X86.Jmp, newBlocks[3].Block);
		}

		// Compare low dwords
		newBlocks[1].AppendInstruction(X86.Cmp32, null, op1L, op2L);
		newBlocks[1].AppendInstruction(X86.Branch, condition.GetUnsigned(), newBlocks[2].Block);
		newBlocks[1].AppendInstruction(X86.Jmp, newBlocks[3].Block);

		// Success
		newBlocks[2].AppendInstruction(X86.Mov32, result, Operand.Constant32_1);
		newBlocks[2].AppendInstruction(X86.Jmp, nextBlock.Block);

		// Failed
		newBlocks[3].AppendInstruction(X86.Mov32, result, Operand.Constant32_0);
		newBlocks[3].AppendInstruction(X86.Jmp, nextBlock.Block);
	}
}
