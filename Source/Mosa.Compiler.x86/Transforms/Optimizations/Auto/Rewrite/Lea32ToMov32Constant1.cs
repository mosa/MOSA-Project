// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.Rewrite;

[Transform("x86.Optimizations.Auto.Rewrite")]
public sealed class Lea32ToMov32Constant1 : BaseTransform
{
	public Lea32ToMov32Constant1() : base(X86.Lea32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.GetOperand(3);

		context.SetInstruction(X86.Mov32, result, t1);
	}
}
