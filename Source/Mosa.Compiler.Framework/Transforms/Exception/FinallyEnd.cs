// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// FinallyEnd
/// </summary>
public sealed class FinallyEnd : BaseExceptionTransform
{
	public FinallyEnd() : base(IRInstruction.FinallyEnd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var exceptionManager = transform.GetManager<ExceptionManager>();

		var naturalBlock = TraverseBackToNativeBlock(context.Block);
		var handler = FindImmediateExceptionHandler(transform, naturalBlock.Label);

		var handlerBlock = transform.BasicBlocks.GetByLabel(handler.HandlerStart);

		var exceptionVirtualRegister = exceptionManager.ExceptionVirtualRegisters[handlerBlock];
		var leaveTargetRegister = exceptionManager.LeaveTargetVirtualRegisters[handlerBlock];

		List<BasicBlock> targets = null;

		// Collect leave target from current block
		var next = FindNextEnclosingFinallyHandler(transform, handler);

		foreach (var target in exceptionManager.LeaveTargets)
		{
			if (target.Label <= naturalBlock.Label)
				continue;

			if (next != null && !next.IsLabelWithinTry(target.Label))
				continue;

			if (targets == null)
				targets = new List<BasicBlock>();

			targets.Add(target);
		}

		var targetcount = targets == null ? 2 : targets.Count + 2;

		var newBlocks = transform.CreateNewBlockContexts(targetcount, context.Label);
		var exceptionCallBlock = newBlocks[0];

		context.SetInstruction(IRInstruction.BranchObject, ConditionCode.NotEqual, null, exceptionVirtualRegister, Operand.NullObject, exceptionCallBlock.Block);
		context.AppendInstruction(IRInstruction.Jmp, newBlocks[1].Block);

		exceptionCallBlock.AppendInstruction(IRInstruction.MoveObject, transform.ExceptionRegister, exceptionVirtualRegister);
		exceptionCallBlock.AppendInstruction(IRInstruction.CallStatic, null, Operand.CreateLabel(exceptionManager.ExceptionHandler, transform.Is32BitPlatform));

		transform.MethodScanner.MethodInvoked(exceptionManager.ExceptionHandler, transform.Method);

		if (targets != null)
		{
			for (var i = 0; i < targets.Count; i++)
			{
				var target = targets[i];
				var conditionBlock = newBlocks[i + 1];

				conditionBlock.AppendInstruction(transform.BranchInstruction, ConditionCode.Equal, null, leaveTargetRegister, Operand.CreateConstant32(target.Label), target);
				conditionBlock.AppendInstruction(IRInstruction.Jmp, newBlocks[i + 2].Block);
			}
		}

		var finallyCallBlock = newBlocks[targetcount - 1];

		if (next != null)
		{
			finallyCallBlock.AppendInstruction(IRInstruction.MoveObject, transform.ExceptionRegister, Operand.NullObject);
			finallyCallBlock.AppendInstruction(IRInstruction.MoveObject, transform.LeaveTargetRegister, leaveTargetRegister);
			finallyCallBlock.AppendInstruction(IRInstruction.Jmp, transform.BasicBlocks.GetByLabel(next.HandlerStart));
		}
		else
		{
			// should be an unreachable block
		}
	}
}
