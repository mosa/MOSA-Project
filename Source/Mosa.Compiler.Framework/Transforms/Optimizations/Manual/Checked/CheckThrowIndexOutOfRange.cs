// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Checked;

public sealed class CheckThrowIndexOutOfRange : BaseTransform
{
	public CheckThrowIndexOutOfRange() : base(IRInstruction.CheckThrowIndexOutOfRange, TransformType.Manual | TransformType.Transform, true)
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
			context.SetInstruction(IRInstruction.ThrowIndexOutOfRange);
		}
	}
}
