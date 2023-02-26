// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding;

public sealed class Compare32x64 : BaseTransform
{
	public Compare32x64() : base(IRInstruction.Compare32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		return IsNormal(context.ConditionCode);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var compare = Compare32(context);

		var e1 = transform.CreateConstant(BoolTo64(compare));

		context.SetInstruction(IRInstruction.Move64, context.Result, e1);
	}
}
