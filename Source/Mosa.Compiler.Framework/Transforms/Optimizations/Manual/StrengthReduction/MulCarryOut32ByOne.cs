// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulCarryOut32ByOne
/// </summary>
public sealed class MulCarryOut32ByOne : BaseTransform
{
	public MulCarryOut32ByOne() : base(Framework.IR.MulCarryOut32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;

		context.SetInstruction(Framework.IR.Move32, result, t1);
		context.AppendInstruction(Framework.IR.Move32, result2, Operand.Constant32_1);
	}
}
