﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class Compare32x32Same : BaseTransformation
	{
		public Compare32x32Same() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			var condition = context.ConditionCode;

			return (condition == ConditionCode.Equal || condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual || condition == ConditionCode.UnsignedLessOrEqual || condition == ConditionCode.LessOrEqual);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Move32, context.Result, transformContext.CreateConstant(1));
		}
	}
}
