// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class ArithShiftRight64ZeroValue : BaseTransform
{
	public ArithShiftRight64ZeroValue() : base(IR.ArithShiftRight64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.CreateConstant(To64(0));

		context.SetInstruction(IR.Move64, result, e1);
	}
}
