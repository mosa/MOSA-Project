// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class ArithShiftRight64 : BaseTransform
{
	public ArithShiftRight64() : base(IR.ArithShiftRight64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(ArithmeticShiftRight64(To64(t1), ToSigned64(t2)));

		context.SetInstruction(IR.Move64, result, e1);
	}
}
