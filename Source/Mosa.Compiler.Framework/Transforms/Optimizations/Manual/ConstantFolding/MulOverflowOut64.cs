// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulOverflowOut64
/// </summary>
public sealed class MulOverflowOut64 : BaseTransform
{
	public MulOverflowOut64() : base(IR.MulOverflowOut64, TransformType.Auto | TransformType.Optimization, 100, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplySignedOverflow(context.Operand1.ConstantSigned64, context.Operand2.ConstantSigned64))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(MulSigned64(ToSigned64(t1), ToSigned64(t2)));

		context.SetInstruction(IR.Move64, result, e1);
		context.AppendInstruction(IR.Move64, result2, Operand.Constant64_1);
	}
}
