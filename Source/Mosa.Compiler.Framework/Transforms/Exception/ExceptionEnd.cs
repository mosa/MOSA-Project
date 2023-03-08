using System.Diagnostics;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// ExceptionEnd
/// </summary>
public sealed class ExceptionEnd : BaseExceptionTransform
{
	public ExceptionEnd() : base(IRInstruction.ExceptionEnd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		//var exceptionManager = transform.GetManager<ExceptionManager>();

		var label = TraverseBackToNativeBlock(context.Block).Label;
		var target = context.BranchTargets[0];

		var immediate = FindImmediateExceptionHandler(transform, label);

		Debug.Assert(immediate != null);
		Debug.Assert(immediate.ExceptionHandlerType == ExceptionHandlerType.Exception);

		var handler = FindNextEnclosingFinallyHandler(transform, immediate);

		if (handler == null)
		{
			context.SetInstruction(IRInstruction.Jmp, target);
			return;
		}

		var handlerBlock = transform.BasicBlocks.GetByLabel(handler.HandlerStart);

		context.SetInstruction(IRInstruction.MoveObject, transform.LeaveTargetRegister, transform.CreateConstant32(target.Label));
		context.AppendInstruction(IRInstruction.MoveObject, transform.ExceptionRegister, transform.NullOperand);
		context.AppendInstruction(IRInstruction.Jmp, handlerBlock);
	}
}
