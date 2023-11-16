// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulCarryOut64ByZero
/// </summary>
public sealed class MulCarryOut64ByZero : BaseTransform
{
	public MulCarryOut64ByZero() : base(IR.MulCarryOut64, TransformType.Manual | TransformType.Optimization, true)
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

		context.SetInstruction(IR.Move64, result, Operand.Constant64_0);
		context.AppendInstruction(IR.Move64, result2, Operand.Constant64_1);
	}
}
