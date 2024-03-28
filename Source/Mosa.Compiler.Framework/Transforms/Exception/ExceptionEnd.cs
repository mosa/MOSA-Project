// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// ExceptionEnd
/// </summary>
public sealed class ExceptionEnd : BaseExceptionTransform
{
	public ExceptionEnd() : base(IR.ExceptionEnd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		//var exceptionManager = transform.GetManager<ExceptionManager>();

		var label = TraverseBackToNativeBlock(context.Block).Label;
		var target = context.BranchTarget1;

		var immediate = FindImmediateExceptionHandler(transform, label);

		Debug.Assert(immediate != null);
		Debug.Assert(immediate.ExceptionHandlerType == ExceptionHandlerType.Exception);

		var handler = FindNextEnclosingFinallyHandler(transform, immediate);

		if (handler == null || handler.HandlerStart >= target.Label)
		{
			context.SetInstruction(IR.Jmp, target);
			return;
		}

		var handlerBlock = transform.BasicBlocks.GetByLabel(handler.HandlerStart);

		context.SetInstruction(IR.MoveObject, transform.LeaveTargetRegister, Operand.CreateConstant32(target.Label));
		context.AppendInstruction(IR.MoveObject, transform.ExceptionRegister, Operand.NullObject);
		context.AppendInstruction(IR.Jmp, handlerBlock);
	}
}
