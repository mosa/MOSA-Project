﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class Compare32x64NotSame : BaseTransformation
	{
		public Compare32x64NotSame() : base(IRInstruction.Compare32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			var condition = context.ConditionCode;

			return (condition == ConditionCode.NotEqual || condition == ConditionCode.GreaterThan || condition == ConditionCode.LessThan || condition == ConditionCode.UnsignedGreaterThan || condition == ConditionCode.UnsignedLessThan);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var operand1 = transformContext.CreateConstant(0);
			context.SetInstruction(IRInstruction.Move64, context.Result, operand1);
		}
	}
}
