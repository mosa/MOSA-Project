// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// Sar32ZeroValue
/// </summary>
[Transform("x86.Optimizations.Auto.StrengthReduction")]
public sealed class Sar32ZeroValue : BaseTransform
{
	public Sar32ZeroValue() : base(X86.Sar32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		if (AreStatusFlagUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0);

		context.SetInstruction(X86.Mov32, result, c1);
	}
}
