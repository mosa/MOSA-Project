// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulCarryOut32
/// </summary>
public sealed class MulCarryOut32 : BaseTransform
{
	public MulCarryOut32() : base(IR.MulCarryOut32, TransformType.Auto | TransformType.Optimization, 100, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplyUnsignedCarry(context.Operand1.ConstantUnsigned32, context.Operand2.ConstantUnsigned32))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(MulUnsigned32(To32(t1), To32(t2)));

		context.SetInstruction(IR.Move32, result, e1);
		context.AppendInstruction(IR.Move32, result2, Operand.Constant32_1);
	}
}
