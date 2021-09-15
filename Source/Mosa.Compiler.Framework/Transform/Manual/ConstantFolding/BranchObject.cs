// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantFolding
{
	public sealed class BranchObject : BaseTransformation
	{
		public BranchObject() : base(IRInstruction.BranchObject)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: return true;
				case ConditionCode.NotEqual: return true;
				case ConditionCode.GreaterOrEqual: return true;
				case ConditionCode.Greater: return true;
				case ConditionCode.LessOrEqual: return true;
				case ConditionCode.Less: return true;
				case ConditionCode.UnsignedGreater: return true;
				case ConditionCode.UnsignedGreaterOrEqual: return true;
				case ConditionCode.UnsignedLess: return true;
				case ConditionCode.UnsignedLessOrEqual: return true;
				default: return false;
			}
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var target = context.BranchTargets[0];
			var block = context.Block;

			if (!Compare64(context))
			{
				context.SetNop();
			}
			else
			{
				context.SetInstruction(IRInstruction.Jmp, target);

				// rest of instructions in block are never used
				context.GotoNext();
				while (!context.IsBlockEndInstruction)
				{
					if (!context.IsEmptyOrNop)
					{
						context.SetNop();
					}
					context.GotoNext();
				}
			}

			TransformContext.RemoveBlockFromPHIInstructions(block, target);
		}
	}
}
