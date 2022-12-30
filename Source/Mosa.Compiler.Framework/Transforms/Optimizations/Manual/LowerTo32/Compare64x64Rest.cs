// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class Compare64x64Rest : BaseTransformation
	{
		public Compare64x64Rest() : base(IRInstruction.Compare64x64, TransformationType.Manual | TransformationType.Optimization, true)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode == ConditionCode.Equal || context.ConditionCode == ConditionCode.NotEqual)
				return false;

			if (transform.IsInSSAForm)
				return false;

			return transform.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.ConditionCode != ConditionCode.Equal);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			transform.SplitLongOperand(result, out Operand resultLow, out Operand resultHigh);

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = transform.Split(context);
			var newBlocks = transform.CreateNewBlockContexts(5, context.Label);

			TransformContext.UpdatePhiTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

			var op0Low = transform.AllocateVirtualRegister32();
			var op0High = transform.AllocateVirtualRegister32();
			var op1Low = transform.AllocateVirtualRegister32();
			var op1High = transform.AllocateVirtualRegister32();
			var tempLow = transform.AllocateVirtualRegister32();

			context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
			context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
			context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
			context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

			// Compare high
			context.AppendInstruction(IRInstruction.Branch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
			context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

			newBlocks[0].AppendInstruction(IRInstruction.Branch32, branch, null, op0High, op1High, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Compare low
			newBlocks[1].AppendInstruction(IRInstruction.Branch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
			newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

			// Success
			newBlocks[2].AppendInstruction(IRInstruction.Move32, tempLow, transform.CreateConstant((uint)1));
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Failed
			newBlocks[3].AppendInstruction(IRInstruction.Move32, tempLow, transform.ConstantZero32);
			newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Exit
			newBlocks[4].AppendInstruction(IRInstruction.Move32, resultLow, tempLow);
			newBlocks[4].AppendInstruction(IRInstruction.Move32, resultHigh, transform.ConstantZero32);
			newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
		}
	}
}
