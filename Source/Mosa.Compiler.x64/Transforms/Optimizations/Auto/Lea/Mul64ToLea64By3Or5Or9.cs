// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.Lea;

public sealed class Mul64ToLea64By3Or5Or9 : BaseTransform
{
	public Mul64ToLea64By3Or5Or9() : base(X64.Mul64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (IsResult2Used(context))
			return false;

		if (IsZero(context.Operand1))
			return false;

		if (!Contains(context.Operand2, 3, 5, 9))
			return false;

		if (AreAnyStatusFlagsUsed(context))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(Sub64(To64(t2), 1));
		var e2 = Operand.Constant64_0;

		context.SetInstruction(X64.Lea64, result, t1, t1, e1, e2);
	}
}
