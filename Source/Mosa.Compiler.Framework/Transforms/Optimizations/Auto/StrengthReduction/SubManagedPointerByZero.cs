// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// SubManagedPointerByZero
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class SubManagedPointerByZero : BaseTransform
{
	public SubManagedPointerByZero() : base(IRInstruction.SubManagedPointer, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		context.SetInstruction(IRInstruction.MoveManagedPointer, result, t1);
	}
}
