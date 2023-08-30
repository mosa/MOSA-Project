// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Standard;

[Transform("x64.Optimizations.Manual.Standard")]
public sealed class Mov32ToXor32 : BaseTransform
{
	public Mov32ToXor32() : base(X64.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		if (AreStatusFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X64.Xor32, context.Result, context.Result, context.Result);
	}
}
