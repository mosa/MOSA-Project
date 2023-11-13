// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// SubCarryOut32ByZero
/// </summary>
public sealed class SubCarryOut32ByZero : BaseTransform
{
	public SubCarryOut32ByZero() : base(Framework.IR.SubCarryOut32, TransformType.Manual | TransformType.Optimization, true)
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
		var result2 = context.Result2;
		var operand1 = context.Operand1;

		context.SetInstruction(Framework.IR.Move32, result, operand1);
		context.AppendInstruction(Framework.IR.Move32, result2, Operand.Constant32_0);
	}
}
