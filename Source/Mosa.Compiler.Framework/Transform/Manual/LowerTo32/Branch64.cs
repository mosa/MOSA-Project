// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class Branch64 : BaseTransformation
	{
		public Branch64() : base(IRInstruction.Branch64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.Block.NextBlocks.Count == 1)
				return false;

			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			//Debug.Assert(context.ConditionCode != ConditionCode.Equal);

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var target = context.BranchTargets[0];

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = transformContext.Split(context);
			var newBlocks = transformContext.CreateNewBlockContexts(4, context.Label);

			// no branch
			TransformContext.UpdatePHIInstructionTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

			// Branch
			TransformContext.UpdatePHIInstructionTarget(target, context.Block, newBlocks[3].Block);

			var op0Low = transformContext.AllocateVirtualRegister32();
			var op0High = transformContext.AllocateVirtualRegister32();
			var op1Low = transformContext.AllocateVirtualRegister32();
			var op1High = transformContext.AllocateVirtualRegister32();

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
}
