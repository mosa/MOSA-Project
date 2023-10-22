﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

public sealed class Compare32x32 : BaseTransform
{
	public Compare32x32() : base(IRInstruction.Compare32x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return IsNormal(context.ConditionCode);
	}

	public override void Transform(Context context, Transform transform)
	{
		var compare = Compare32(context.ConditionCode, context.Operand1, context.Operand2);

		var e1 = Operand.CreateConstant(BoolTo32(compare));

		context.SetInstruction(IRInstruction.Move32, context.Result, e1);
	}
}
