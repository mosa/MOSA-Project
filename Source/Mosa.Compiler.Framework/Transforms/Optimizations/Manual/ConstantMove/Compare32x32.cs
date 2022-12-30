// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantMove
{
	public sealed class Compare32x32 : BaseTransformation
	{
		public Compare32x32() : base(IRInstruction.Compare32x32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (IsConstant(context.Operand2))
				return false;

			if (!IsConstant(context.Operand1))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.Compare32x32, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
		}
	}
}
