// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class CompareInt32x32 : BaseTransformation
	{
		public CompareInt32x32() : base(IRInstruction.CompareInt32x32, OperandFilter.ResolvedConstant, OperandFilter.ResolvedConstant)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!(context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant))
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
			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = context.Operand1.ConstantUnsignedLongInteger == context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.NotEqual: compareResult = context.Operand1.ConstantUnsignedLongInteger != context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.GreaterOrEqual: compareResult = context.Operand1.ConstantUnsignedLongInteger >= context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.GreaterThan: compareResult = context.Operand1.ConstantUnsignedLongInteger > context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.LessOrEqual: compareResult = context.Operand1.ConstantUnsignedLongInteger <= context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.LessThan: compareResult = context.Operand1.ConstantUnsignedLongInteger < context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.UnsignedGreaterThan: compareResult = context.Operand1.ConstantUnsignedLongInteger > context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.UnsignedGreaterOrEqual: compareResult = context.Operand1.ConstantUnsignedLongInteger >= context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.UnsignedLessThan: compareResult = context.Operand1.ConstantUnsignedLongInteger < context.Operand2.ConstantUnsignedLongInteger; break;
				case ConditionCode.UnsignedLessOrEqual: compareResult = context.Operand1.ConstantUnsignedLongInteger <= context.Operand2.ConstantUnsignedLongInteger; break;
			}

			SetConstantResult(context, compareResult ? 1 : 0);
		}
	}
}
