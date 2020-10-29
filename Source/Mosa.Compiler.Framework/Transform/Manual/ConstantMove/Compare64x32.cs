﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantMove
{
	public sealed class Compare64x32 : BaseTransformation
	{
		public Compare64x32() : base(IRInstruction.Compare64x32)
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
			context.SetInstruction(IRInstruction.Compare64x32, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
		}
	}
}
