﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class Compare64x64NotSame : BaseTransformation
	{
		public Compare64x64NotSame() : base(IRInstruction.Compare64x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!AreSame(context.Operand1, context.Operand2))
				return false;

			var condition = context.ConditionCode;

			return (condition == ConditionCode.NotEqual || condition == ConditionCode.Greater || condition == ConditionCode.Less || condition == ConditionCode.UnsignedGreater || condition == ConditionCode.UnsignedLess);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Move64, context.Result, transformContext.ConstantZero64);
		}
	}
}
