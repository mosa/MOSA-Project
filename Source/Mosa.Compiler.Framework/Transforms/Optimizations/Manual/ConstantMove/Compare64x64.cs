// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantMove
{
	public sealed class Compare64x64 : BaseTransform
	{
		public Compare64x64() : base(IRInstruction.Compare64x64, TransformType.Manual | TransformType.Optimization)
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
			context.SetInstruction(IRInstruction.Compare64x64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1);
		}
	}
}
