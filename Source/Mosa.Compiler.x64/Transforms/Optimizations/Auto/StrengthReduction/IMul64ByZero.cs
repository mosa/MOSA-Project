// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class IMul64ByZero : BaseTransform
{
	public IMul64ByZero() : base(X64.IMul64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsConstantZero)
			return false;

		if (AreAnyStatusFlagsUsed(context, transform.Window))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0);

		context.SetInstruction(X64.Mov64, result, c1);
	}
}
