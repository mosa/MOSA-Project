// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Simplification;

public sealed class SubCarryOut32CarryNotUsed : BaseTransform
{
	public SubCarryOut32CarryNotUsed() : base(Framework.IR.SubCarryOut32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return !context.Result2.IsUsed;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(Framework.IR.Sub32, context.Result, context.Operand1, context.Operand2);
	}
}
