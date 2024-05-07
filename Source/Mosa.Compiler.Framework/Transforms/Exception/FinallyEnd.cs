// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// FinallyEnd
/// </summary>
public sealed class FinallyEnd : BaseExceptionTransform
{
	public FinallyEnd() : base(IR.FinallyEnd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var naturalBlock = TraverseBackToNativeBlock(context.Block);
		var handler = FindImmediateExceptionHandler(transform, naturalBlock.Label);

		var handlerBlock = transform.BasicBlocks.GetByLabel(handler.HandlerStart);

		var exceptionManager = transform.GetManager<ExceptionManager>();

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

		context.SetInstruction(IR.BranchObject, ConditionCode.NotEqual, null, exceptionVirtualRegister, Operand.NullObject, exceptionCallBlock.Block);
		context.AppendInstruction(IR.Jmp, newBlocks[1].Block);

		exceptionCallBlock.AppendInstruction(IR.MoveObject, transform.ExceptionRegister, exceptionVirtualRegister);
		exceptionCallBlock.AppendInstruction(IR.CallStatic, null, Operand.CreateLabel(exceptionManager.ExceptionHandler, transform.Is32BitPlatform));

		transform.MethodScanner.MethodInvoked(exceptionManager.ExceptionHandler, transform.Method);

		if (targets != null)
		{
			for (var i = 0; i < targets.Count; i++)
			{
				var target = targets[i];
				var conditionBlock = newBlocks[i + 1];

				if (next == null && i == targets.Count - 1)
				{
					conditionBlock.AppendInstruction(IR.Jmp, target);
				}
				else
				{
					conditionBlock.AppendInstruction(transform.BranchInstruction, ConditionCode.Equal, null, leaveTargetRegister, Operand.CreateConstant32(target.Label), target);
					conditionBlock.AppendInstruction(IR.Jmp, newBlocks[i + 2].Block);
				}
			}
		}

		if (next != null)
		{
			var finallyCallBlock = newBlocks[targetcount - 1];

			finallyCallBlock.AppendInstruction(IR.MoveObject, transform.ExceptionRegister, Operand.NullObject);
			finallyCallBlock.AppendInstruction(IR.MoveObject, transform.LeaveTargetRegister, leaveTargetRegister);
			finallyCallBlock.AppendInstruction(IR.Jmp, transform.BasicBlocks.GetByLabel(next.HandlerStart));
		}
	}
}
