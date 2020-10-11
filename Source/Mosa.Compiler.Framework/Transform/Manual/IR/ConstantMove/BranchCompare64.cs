// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.IR.ConstantMove
{
	public sealed class BranchCompare64 : BaseTransformation
	{
		public BranchCompare64() : base(IRInstruction.BranchCompare64)
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
			context.SetInstruction(IRInstruction.BranchCompare64, context.ConditionCode.GetReverse(), context.Result, context.Operand2, context.Operand1, context.BranchTargets[0]);
		}
	}
}
