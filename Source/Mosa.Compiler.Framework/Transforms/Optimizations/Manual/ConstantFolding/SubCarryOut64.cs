// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// AddCarryOut64
/// </summary>
public sealed class SubCarryOut64 : BaseTransform
{
	public SubCarryOut64() : base(IRInstruction.SubCarryOut64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1.ConstantUnsigned64;
		var t2 = context.Operand2.ConstantUnsigned64;

		var e1 = transform.CreateConstant(t1 - t2);
		var carry = IntegerTwiddling.IsSubUnsignedCarry(t1, t2);

		context.SetInstruction(IRInstruction.Move64, result, e1);
		context.AppendInstruction(IRInstruction.Move64, result2, carry ? transform.Constant64_1 : transform.Constant64_0);
	}
}
