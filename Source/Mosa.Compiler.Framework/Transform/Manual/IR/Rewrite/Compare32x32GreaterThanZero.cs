// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Rewrite
{
	public sealed class Compare32x32GreaterThanZero : BaseTransformation
	{
		public Compare32x32GreaterThanZero() : base(IRInstruction.Compare32x32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (context.Operand2.ConstantUnsigned32 != 0)
				return false;

			if (context.ConditionCode != ConditionCode.UnsignedGreaterThan)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.Compare32x32, ConditionCode.NotEqual, context.Result, context.Operand1, context.Operand2);
		}
	}
}
