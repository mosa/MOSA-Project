// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.Simplification
{
	public sealed class Compare32x32NotSame : BaseTransformation
	{
		public Compare32x32NotSame() : base(IRInstruction.Compare32x32)
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
			var operand1 = transformContext.CreateConstant(0);
			context.SetInstruction(IRInstruction.Move32, context.Result, operand1);
		}
	}
}
