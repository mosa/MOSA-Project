// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulOverflowOut64
/// </summary>
public sealed class MulOverflowOut64 : BaseTransform
{
	public MulOverflowOut64() : base(IRInstruction.MulOverflowOut64, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplyOverflow(context.Operand1.ConstantSigned64, context.Operand2.ConstantSigned64))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = transform.CreateConstant(MulSigned64(ToSigned64(t1), ToSigned64(t2)));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
