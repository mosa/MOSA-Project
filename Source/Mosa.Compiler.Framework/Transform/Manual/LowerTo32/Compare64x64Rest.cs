// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class Compare64x64Rest : BaseTransformation
	{
		public Compare64x64Rest() : base(IRInstruction.Compare64x64, true)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (context.ConditionCode == ConditionCode.Equal || context.ConditionCode == ConditionCode.NotEqual)
				return false;

			if (transformContext.IsInSSAForm)
				return false;

			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.ConditionCode != ConditionCode.Equal);

			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			transformContext.SplitLongOperand(result, out Operand resultLow, out Operand resultHigh);

			var branch = context.ConditionCode;
			var branchUnsigned = context.ConditionCode.GetUnsigned();

			var nextBlock = transformContext.Split(context);
			var newBlocks = transformContext.CreateNewBlockContexts(5, context.Label);

			TransformContext.UpdatePHIInstructionTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

			var op0Low = transformContext.AllocateVirtualRegister32();
			var op0High = transformContext.AllocateVirtualRegister32();
			var op1Low = transformContext.AllocateVirtualRegister32();
			var op1High = transformContext.AllocateVirtualRegister32();
			var tempLow = transformContext.AllocateVirtualRegister32();

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
			newBlocks[2].AppendInstruction(IRInstruction.Move32, tempLow, transformContext.CreateConstant((uint)1));
			newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Failed
			newBlocks[3].AppendInstruction(IRInstruction.Move32, tempLow, transformContext.ConstantZero32);
			newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

			// Exit
			newBlocks[4].AppendInstruction(IRInstruction.Move32, resultLow, tempLow);
			newBlocks[4].AppendInstruction(IRInstruction.Move32, resultHigh, transformContext.ConstantZero32);
			newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
		}
	}
}
