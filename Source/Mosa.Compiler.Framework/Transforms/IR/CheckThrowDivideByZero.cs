// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.IR;

public sealed class CheckThrowDivideByZero : BaseTransform
{
	public CheckThrowDivideByZero() : base(IRInstruction.CheckThrowDivideByZero, TransformType.Manual | TransformType.Transform)
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
				context.SetInstruction(IRInstruction.ThrowDivideByZero);
			}
			return;
		}

		var newBlock = transform.CreateNewBlockContexts(1, context.Label)[0];
		var nextBlock = transform.Split(context);

		context.SetInstruction(transform.BranchInstruction, ConditionCode.NotEqual, null, operand1, Operand.Constant32_0, newBlock.Block);
		context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

		newBlock.AppendInstruction(IRInstruction.ThrowDivideByZero);
	}
}
