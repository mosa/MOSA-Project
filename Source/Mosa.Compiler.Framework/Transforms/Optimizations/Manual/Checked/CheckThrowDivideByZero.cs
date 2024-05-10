// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Checked;

public sealed class CheckThrowDivideByZero : BaseTransform
{
	public CheckThrowDivideByZero() : base(IR.CheckThrowDivideByZero, TransformType.Manual | TransformType.Optimization, 100, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand1.IsResolvedConstant;
	}

	public override void Transform(Context context, Transform transform)
	{
		if (context.Operand1.ConstantUnsigned64 == 0)
		{
			context.SetNop();
		}
		else
		{
			context.SetInstruction(IR.ThrowDivideByZero);
		}
	}
}
