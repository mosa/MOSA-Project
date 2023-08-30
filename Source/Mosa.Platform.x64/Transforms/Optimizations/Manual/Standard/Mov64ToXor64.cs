// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Standard;

[Transform("x64.Optimizations.Manual.Standard")]
public sealed class Mov64ToXor64 : BaseTransform
{
	public Mov64ToXor64() : base(X64.Mov64, TransformType.Manual | TransformType.Optimization)
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
		context.SetInstruction(X64.Xor64, context.Result, context.Result, context.Result);
	}
}
