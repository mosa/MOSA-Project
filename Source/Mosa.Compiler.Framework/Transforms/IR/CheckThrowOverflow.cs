// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class CheckThrowOverflow : BaseTransform
{
	public CheckThrowOverflow() : base(IRInstruction.CheckThrowOverflow, TransformType.Manual | TransformType.Transform, true)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var operand1 = context.Operand1;

		var newBlock = transform.CreateNewBlockContexts(1, context.Label)[0];
		var nextBlock = transform.Split(context);

		context.SetInstruction(transform.BranchInstruction, ConditionCode.NotEqual, null, operand1, transform.Constant32_0, newBlock.Block);
		context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		newBlock.AppendInstruction(IRInstruction.ThrowOverflow);
	}
}
