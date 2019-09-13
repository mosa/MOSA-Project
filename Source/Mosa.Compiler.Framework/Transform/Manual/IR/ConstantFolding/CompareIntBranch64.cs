// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.ConstantFolding
{
	public sealed class CompareIntBranch64 : BaseTransformation
	{
		public CompareIntBranch64() : base(IRInstruction.CompareIntBranch64)
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
				case ConditionCode.GreaterThan: return true;
				case ConditionCode.LessOrEqual: return true;
				case ConditionCode.LessThan: return true;
				case ConditionCode.UnsignedGreaterThan: return true;
				case ConditionCode.UnsignedGreaterOrEqual: return true;
				case ConditionCode.UnsignedLessThan: return true;
				case ConditionCode.UnsignedLessOrEqual: return true;
				default: return false;
			}
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			bool compare = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compare = context.Operand1.ConstantUnsigned64 == context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.NotEqual: compare = context.Operand1.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.GreaterOrEqual: compare = context.Operand1.ConstantUnsigned64 >= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.GreaterThan: compare = context.Operand1.ConstantUnsigned64 > context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.LessOrEqual: compare = context.Operand1.ConstantUnsigned64 <= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.LessThan: compare = context.Operand1.ConstantUnsigned64 < context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedGreaterThan: compare = context.Operand1.ConstantUnsigned64 > context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedGreaterOrEqual: compare = context.Operand1.ConstantUnsigned64 >= context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedLessThan: compare = context.Operand1.ConstantUnsigned64 < context.Operand2.ConstantUnsigned64; break;
				case ConditionCode.UnsignedLessOrEqual: compare = context.Operand1.ConstantUnsigned64 <= context.Operand2.ConstantUnsigned64; break;
			}

			if (!compare)
			{
				context.SetInstruction(IRInstruction.Nop);
			}
			else
			{
				var target = context.BranchTargets[0];
				context.SetInstruction(IRInstruction.Jmp, target);

				// rest of instructions in block are never used
				context.GotoNext();
				while (!context.IsBlockEndInstruction)
				{
					if (!context.IsEmptyOrNop)
					{
						context.SetInstruction(IRInstruction.Nop);
					}
					context.GotoNext();
				}
			}
		}
	}
}
