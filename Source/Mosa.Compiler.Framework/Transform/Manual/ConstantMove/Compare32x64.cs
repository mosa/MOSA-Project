﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantMove
{
	public sealed class Compare32x64 : BaseTransformation
	{
		public Compare32x64() : base(IRInstruction.Compare32x64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (IsConstant(context.Operand2))
				return false;

			if (!IsConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Compare32x64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
		}
	}
}
