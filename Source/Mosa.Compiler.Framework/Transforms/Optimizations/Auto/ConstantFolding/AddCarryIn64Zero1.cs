// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class AddCarryIn64Zero1 : BaseTransform
{
	public AddCarryIn64Zero1() : base(IR.AddCarryIn64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsConstantZero)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2;
		var t2 = context.Operand3;

		context.SetInstruction(IR.Add64, result, t1, t2);
	}
}
