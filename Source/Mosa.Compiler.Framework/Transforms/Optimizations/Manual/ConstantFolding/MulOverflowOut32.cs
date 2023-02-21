// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulOverflowOut32
/// </summary>
public sealed class MulOverflowOut32 : BaseTransform
{
	public MulOverflowOut32() : base(IRInstruction.MulOverflowOut32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplySignedOverflow(context.Operand1.ConstantSigned32, context.Operand2.ConstantSigned32))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = transform.CreateConstant(MulSigned32(ToSigned32(t1), ToSigned32(t2)));

		context.SetInstruction(IRInstruction.Move32, result, e1);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_1);
	}
}
