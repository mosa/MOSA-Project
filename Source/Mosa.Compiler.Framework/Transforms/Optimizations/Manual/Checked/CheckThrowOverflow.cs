// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Checked;

public sealed class CheckThrowOverflow : BaseTransform
{
	public CheckThrowOverflow() : base(IRInstruction.CheckThrowOverflow, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand1.IsResolvedConstant;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		if (context.Operand1.ConstantUnsigned64 == 0)
		{
			context.SetNop();
		}
		else
		{
			context.SetInstruction(IRInstruction.ThrowOverflow);
		}
	}
}
