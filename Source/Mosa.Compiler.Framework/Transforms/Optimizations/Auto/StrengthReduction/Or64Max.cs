// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// Or64Max
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Or64Max : BaseTransform
{
	public Or64Max() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0xFFFFFFFFFFFFFFFF)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0xFFFFFFFFFFFFFFFF);

		context.SetInstruction(Framework.IR.Move64, result, c1);
	}
}

/// <summary>
/// Or64Max_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Or64Max_v1 : BaseTransform
{
	public Or64Max_v1() : base(Framework.IR.Or64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFFFFFFFFFF)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(0xFFFFFFFFFFFFFFFF);

		context.SetInstruction(Framework.IR.Move64, result, c1);
	}
}
