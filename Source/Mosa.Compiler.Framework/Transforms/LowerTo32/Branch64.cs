// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Branch64 : BaseLowerTo32Transform
{
	private readonly Branch64Extends branch64Extends = new Branch64Extends(); // BUG?

	public Branch64() : base(IRInstruction.Branch64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.Block.NextBlocks.Count == 1)
			return false;

		if (branch64Extends.Match(context, transform))
			return false;

		return base.Match(context, transform);
	}

	public override void Transform(Context context, Transform transform)
	{
		//Debug.Assert(context.ConditionCode != ConditionCode.Equal);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var target = context.BranchTargets[0];

		var branch = context.ConditionCode;
		var branchUnsigned = context.ConditionCode.GetUnsigned();

		var nextBlock = transform.Split(context);
		var newBlocks = transform.CreateNewBlockContexts(4, context.Label);

		// no branch
		Framework.Transform.UpdatePhiTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

		// Branch
		Framework.Transform.UpdatePhiTarget(target, context.Block, newBlocks[3].Block);

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
		context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
		context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

		// Compare high (equal)
		context.AppendInstruction(IRInstruction.Branch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
		context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

		// Compare high
		newBlocks[0].AppendInstruction(IRInstruction.Branch32, branch, null, op0High, op1High, newBlocks[3].Block);
		newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[2].Block);

		// Compare low
		newBlocks[1].AppendInstruction(IRInstruction.Branch32, branchUnsigned, null, op0Low, op1Low, newBlocks[3].Block);
		newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[2].Block);

		// No branch
		newBlocks[2].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		// Branch
		newBlocks[3].AppendInstruction(IRInstruction.Jmp, target);
	}
}
