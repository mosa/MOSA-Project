// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite;

public sealed class Compare64x64Combine64x32 : BaseTransform
{
	public Compare64x64Combine64x32() : base(IRInstruction.Compare64x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.Equal && context.ConditionCode != ConditionCode.NotEqual)
			return false;

		if (IsResolvedConstant(context.Operand1))
			return false;

		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (context.Operand2.ConstantUnsigned32 != 0)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (context.Operand1.Definitions[0].Instruction != IRInstruction.Compare64x32)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var node2 = context.Operand1.Definitions[0];
		var conditionCode = context.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();

		context.SetInstruction(IRInstruction.Compare64x32, conditionCode, context.Result, node2.Operand1, node2.Operand2);
	}
}
