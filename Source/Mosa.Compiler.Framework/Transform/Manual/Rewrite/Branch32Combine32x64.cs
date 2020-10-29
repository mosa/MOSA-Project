﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.Rewrite
{
	public sealed class Branch32Combine32x64 : BaseTransformation
	{
		public Branch32Combine32x64() : base(IRInstruction.Branch32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
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

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Compare32x64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var node2 = context.Operand1.Definitions[0];
			var conditionCode = context.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();

			context.SetInstruction(IRInstruction.Branch32, conditionCode, null, node2.Operand1, node2.Operand2, context.BranchTargets[0]);
		}
	}
}
