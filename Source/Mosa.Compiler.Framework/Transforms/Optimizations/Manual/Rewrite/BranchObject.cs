// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Rewrite
{
	public sealed class BranchObject : BaseTransform
	{
		public BranchObject() : base(IRInstruction.BranchObject, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.ConditionCode != ConditionCode.Equal && context.ConditionCode != ConditionCode.NotEqual)
				return false;

			if (IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (context.Operand2.ConstantUnsigned32 != 0)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.CompareObject)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var node2 = context.Operand1.Definitions[0];
			var conditionCode = context.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();

			context.SetInstruction(IRInstruction.BranchObject, conditionCode, null, node2.Operand1, node2.Operand2, context.BranchTargets[0]);
		}
	}
}
