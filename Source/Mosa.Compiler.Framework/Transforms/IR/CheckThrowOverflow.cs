// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class CheckThrowOverflow : BaseTransform
{
	public CheckThrowOverflow() : base(Framework.IR.CheckThrowOverflow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var operand1 = context.Operand1;

		// optional - built-in optimization
		if (operand1.IsResolvedConstant)
		{
			if (operand1.ConstantUnsigned64 == 0)
			{
				context.SetNop();
			}
			else
			{
				context.SetInstruction(Framework.IR.ThrowOverflow);
			}
			return;
		}

		var newBlock = transform.CreateNewBlockContexts(1, context.Label)[0];
		var nextBlock = transform.Split(context);

		context.SetInstruction(transform.BranchInstruction, ConditionCode.NotEqual, null, operand1, Operand.Constant32_0, newBlock.Block);
		context.AppendInstruction(Framework.IR.Jmp, nextBlock.Block);

		newBlock.AppendInstruction(Framework.IR.ThrowOverflow);
	}
}
