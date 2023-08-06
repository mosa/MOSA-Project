﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Compare64x32RestInSSA : BaseLower32Transform
{
	public Compare64x32RestInSSA() : base(IRInstruction.Compare64x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (context.ConditionCode is ConditionCode.Equal or ConditionCode.NotEqual)
			return false;

		if (!transform.IsInSSAForm)
			return false;

		return transform.IsLowerTo32;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var branch = context.ConditionCode;
		var branchUnsigned = context.ConditionCode.GetUnsigned();

		var nextBlock = transform.Split(context);
		var newBlocks = transform.CreateNewBlockContexts(5, context.Label);

		TransformContext.UpdatePhiTargets(nextBlock.Block.NextBlocks, context.Block, nextBlock.Block);

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, op0Low, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, op0High, operand1);
		context.AppendInstruction(IRInstruction.GetLow32, op1Low, operand2);
		context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand2);

		// Compare high
		context.AppendInstruction(IRInstruction.Branch32, ConditionCode.Equal, null, op0High, op1High, newBlocks[1].Block);
		context.AppendInstruction(IRInstruction.Jmp, newBlocks[0].Block);

		// Branch if check already gave results
		if (branch == ConditionCode.Equal)
		{
			newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
		}
		else
		{
			newBlocks[0].AppendInstruction(IRInstruction.Branch32, branch, null, op0High, op1High, newBlocks[2].Block);
			newBlocks[0].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);
		}

		// Compare low
		newBlocks[1].AppendInstruction(IRInstruction.Branch32, branchUnsigned, null, op0Low, op1Low, newBlocks[2].Block);
		newBlocks[1].AppendInstruction(IRInstruction.Jmp, newBlocks[3].Block);

		// Success
		newBlocks[2].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

		// Failed
		newBlocks[3].AppendInstruction(IRInstruction.Jmp, newBlocks[4].Block);

		// Exit
		newBlocks[4].AppendInstruction(IRInstruction.Phi32, result, Operand.CreateConstant((uint)1), Operand.Constant32_0);
		newBlocks[4].PhiBlocks = new List<BasicBlock>(2) { newBlocks[2].Block, newBlocks[3].Block };
		newBlocks[4].AppendInstruction(IRInstruction.Jmp, nextBlock.Block);
	}
}
