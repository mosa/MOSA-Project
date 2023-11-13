// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// TryEnd
/// </summary>
public sealed class TryEnd : BaseExceptionTransform
{
	public TryEnd() : base(IR.TryEnd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var label = TraverseBackToNativeBlock(context.Block).Label;
		var immediate = FindImmediateExceptionHandler(transform, label);
		var target = context.BranchTargets[0];

		Debug.Assert(immediate != null);

		if (immediate.ExceptionHandlerType == ExceptionHandlerType.Finally)
		{
			context.SetInstruction(IR.MoveObject, transform.LeaveTargetRegister, Operand.CreateConstant32(target.Label));
			context.AppendInstruction(IR.MoveObject, transform.ExceptionRegister, Operand.NullObject);
			context.AppendInstruction(IR.Jmp, transform.BasicBlocks.GetByLabel(immediate.HandlerStart));
			return;
		}

		// fixme --- jump to target unless, there is a finally before it.

		var next = FindNextEnclosingFinallyHandler(transform, immediate);

		if (next != null && next.HandlerEnd > immediate.HandlerEnd)
		{
			context.SetInstruction(IR.MoveObject, transform.LeaveTargetRegister, Operand.CreateConstant32(target.Label));
			context.AppendInstruction(IR.MoveObject, transform.ExceptionRegister, Operand.NullObject);
			context.AppendInstruction(IR.Jmp, transform.BasicBlocks.GetByLabel(next.HandlerStart));
			return;
		}

		context.SetInstruction(IR.Jmp, target);
	}
}
