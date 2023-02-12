// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulCarryOut64ByZero
/// </summary>
public sealed class MulCarryOut64ByZero : BaseTransform
{
	public MulCarryOut64ByZero() : base(IRInstruction.MulCarryOut64, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		context.SetInstruction(IRInstruction.Move64, result, transform.Constant64_0);
		context.AppendInstruction(IRInstruction.Move64, result2, transform.Constant64_1);
	}
}

/// <summary>
/// MulUnsigned64ByZero_v1
/// </summary>
public sealed class MulUnsigned64ByZero_v1 : BaseTransform
{
	public MulUnsigned64ByZero_v1() : base(IRInstruction.MulUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var e1 = transform.CreateConstant(To64(0));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
