// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

/// <summary>
/// MulCarryOut32
/// </summary>
public sealed class MulCarryOut32 : BaseTransform
{
	public MulCarryOut32() : base(IRInstruction.MulCarryOut32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (IntegerTwiddling.IsMultiplyOverflow(context.Operand1.ConstantUnsigned32, context.Operand2.ConstantUnsigned32))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = transform.CreateConstant(MulUnsigned32(To32(t1), To32(t2)));

		context.SetInstruction(IRInstruction.Move32, result, e1);
	}
}
