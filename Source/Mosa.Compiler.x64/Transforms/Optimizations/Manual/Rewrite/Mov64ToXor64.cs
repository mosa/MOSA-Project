// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Rewrite;

[Transform("x64.Optimizations.Manual.Rewrite")]
public sealed class Mov64ToXor64 : BaseTransform
{
	public Mov64ToXor64() : base(X64.Mov64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		if (AreAnyStatusFlagsUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Xor64, context.Result, context.Result, context.Result);
	}
}
