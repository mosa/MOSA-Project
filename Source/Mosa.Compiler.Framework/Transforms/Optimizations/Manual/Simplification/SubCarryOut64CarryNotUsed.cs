// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class SubCarryOut64CarryNotUsed : BaseTransform
{
	public SubCarryOut64CarryNotUsed() : base(IRInstruction.SubCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return !context.Result2.IsUsed;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.Sub64, context.Result, context.Operand1, context.Operand2);
	}
}
