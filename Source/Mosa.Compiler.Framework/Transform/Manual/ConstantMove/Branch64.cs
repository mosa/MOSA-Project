// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantMove
{
	public sealed class Branch64 : BaseTransformation
	{
		public Branch64() : base(IRInstruction.Branch64)
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
			context.SetInstruction(IRInstruction.Branch64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1, context.BranchTargets[0]);
		}
	}
}
