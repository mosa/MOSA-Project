// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulCarryOut64ByOne
/// </summary>
public sealed class MulCarryOut64ByOne : BaseTransform
{
	public MulCarryOut64ByOne() : base(IRInstruction.MulCarryOut64, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;

		context.SetInstruction(IRInstruction.Move64, result, t1);
		context.AppendInstruction(IRInstruction.Move64, result2, transform.Constant64_1);
	}
}

/// <summary>
/// MulCarryOut64ByOne_v1
/// </summary>
public sealed class MulCarryOut64ByOne_v1 : BaseTransform
{
	public MulCarryOut64ByOne_v1() : base(IRInstruction.MulCarryOut64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand2;

		context.SetInstruction(IRInstruction.Move64, result, t1);
		context.AppendInstruction(IRInstruction.Move64, result2, transform.Constant64_1);
	}
}
