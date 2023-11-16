// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// FinallyStart
/// </summary>
public sealed class FinallyStart : BaseExceptionTransform
{
	public FinallyStart() : base(IR.FinallyStart, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var exceptionManager = transform.GetManager<ExceptionManager>();

		// Remove from header blocks
		transform.BasicBlocks.RemoveHeaderBlock(context.Block);

		var exceptionVirtualRegister = context.Result;
		var leaveTargetVirtualRegister = context.Result2;

		context.SetInstruction(IR.KillAll);
		context.AppendInstruction(IR.Gen, transform.ExceptionRegister);
		context.AppendInstruction(IR.Gen, transform.LeaveTargetRegister);

		context.AppendInstruction(IR.MoveObject, exceptionVirtualRegister, transform.ExceptionRegister);
		context.AppendInstruction(IR.MoveObject, leaveTargetVirtualRegister, transform.LeaveTargetRegister);

		exceptionManager.ExceptionVirtualRegisters.Add(context.Block, exceptionVirtualRegister);
		exceptionManager.LeaveTargetVirtualRegisters.Add(context.Block, leaveTargetVirtualRegister);
	}
}
