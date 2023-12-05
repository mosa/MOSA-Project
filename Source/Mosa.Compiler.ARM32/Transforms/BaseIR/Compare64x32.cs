// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Compare64x32
/// </summary>
public sealed class Compare64x32 : BaseIRTransform
{
	public Compare64x32() : base(IR.Compare64x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//Compare64x64(context);

		Debug.Assert(context.Operand1 != null && context.Operand2 != null);
		Debug.Assert(context.Result.IsVirtualRegister);

		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		var result = context.Result;
		var condition = context.ConditionCode;

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);
		op2L = MoveConstantToRegister(transform, context, op2L);
		op2H = MoveConstantToRegister(transform, context, op2L);

		var nextBlock = transform.Split(context);
		var newBlocks = transform.CreateNewBlockContexts(4, context.Label);

		if (op1H.IsResolvedConstant && op2H.IsResolvedConstant)
		{
			// high dwords are constants
			context.SetInstruction(ARM32.B, op1H.ConstantUnsigned32 == op2H.ConstantUnsigned32 ? newBlocks[1].Block : newBlocks[0].Block);
		}
		else
		{
			// Compare high dwords
			context.SetInstruction(ARM32.Cmp, null, op1H, op2H);
			context.AppendInstruction(ARM32.B, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(ARM32.B, ConditionCode.Always, newBlocks[0].Block);
		}

		// Branch if check already gave results
		if (condition == ConditionCode.Equal)
		{
			newBlocks[0].AppendInstruction(ARM32.B, newBlocks[3].Block);
		}
		else
		{
			// Branch if check already gave results
			newBlocks[0].AppendInstruction(ARM32.B, condition, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(ARM32.B, ConditionCode.Always, newBlocks[3].Block);
		}

		// Compare low dwords
		newBlocks[1].AppendInstruction(ARM32.Cmp, null, op1L, op2L);
		newBlocks[1].AppendInstruction(ARM32.B, condition.GetUnsigned(), newBlocks[2].Block);
		newBlocks[1].AppendInstruction(ARM32.B, ConditionCode.Always, newBlocks[3].Block);

		// Success
		newBlocks[2].AppendInstruction(ARM32.Mov, result, Operand.Constant32_1);
		newBlocks[2].AppendInstruction(ARM32.B, ConditionCode.Always, nextBlock.Block);

		// Failed
		newBlocks[3].AppendInstruction(ARM32.Mov, result, Operand.Constant32_0);
		newBlocks[3].AppendInstruction(ARM32.B, ConditionCode.Always, nextBlock.Block);
	}
}
