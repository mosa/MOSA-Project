// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Rewrite;

[Transform("x86.Optimizations.Manual.Rewrite")]
public sealed class Lea32ToMov32 : BaseTransform
{
	public Lea32ToMov32() : base(X86.Lea32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		if (!context.Operand4.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X86.Mov32, context.Result, context.Operand1);
	}
}
