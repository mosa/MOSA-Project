// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Standard;

[Transform("x64.Optimizations.Manual.Standard")]
public sealed class Cmp32ToTest32 : BaseTransform
{
	public Cmp32ToTest32() : base(X64.Cmp32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Test32, null, context.Operand1, context.Operand1);
	}
}
