// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Branch64
/// </summary>
public sealed class Branch64 : BaseIRTransform
{
	public Branch64() : base(IR.Branch64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		var target = context.BranchTarget1;
		var condition = context.ConditionCode;

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);
		op2L = MoveConstantToRegister(transform, context, op2L);
		op2H = MoveConstantToRegister(transform, context, op2L);

		var nextBlock = transform.Split(context);
		var newBlocks = transform.CreateNewBlockContexts(2, context.Label);

		if (op1H.IsResolvedConstant && op2H.IsResolvedConstant)
		{
			// high dwords are constants
			context.SetInstruction(ARM32.B, ConditionCode.Always, op1H.ConstantUnsigned32 == op2H.ConstantUnsigned32 ? newBlocks[1].Block : newBlocks[0].Block);
		}
		else
		{
			// Compare high dwords
			context.SetInstruction(ARM32.Cmp, null, op1H, op2H);
			context.AppendInstruction(ARM32.B, ConditionCode.Equal, newBlocks[1].Block);
			context.AppendInstruction(ARM32.B, ConditionCode.Always, newBlocks[0].Block);
		}

		// Branch if check already gave results
		newBlocks[0].AppendInstruction(ARM32.B, condition, target);
		newBlocks[0].AppendInstruction(ARM32.B, ConditionCode.Always, nextBlock.Block);

		// Compare low dwords
		newBlocks[1].AppendInstruction(ARM32.Cmp, null, op1L, op2L);
		newBlocks[1].AppendInstruction(ARM32.B, condition.GetUnsigned(), target);
		newBlocks[1].AppendInstruction(ARM32.B, ConditionCode.Always, nextBlock.Block);
	}
}
