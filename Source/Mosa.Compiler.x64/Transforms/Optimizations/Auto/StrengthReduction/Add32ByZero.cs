// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class Add32ByZero : BaseTransform
{
	public Add32ByZero() : base(X64.Add32, TransformType.Auto | TransformType.Optimization)
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

		var t1 = context.Operand1;

		context.SetInstruction(X64.Mov32, result, t1);
	}
}
