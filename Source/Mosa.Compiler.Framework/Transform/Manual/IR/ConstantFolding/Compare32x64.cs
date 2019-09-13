// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Transform.Manual.IR.ConstantFolding
{
	public sealed class Compare32x64 : BaseTransformation
	{
		public Compare32x64() : base(IRInstruction.Compare32x64)
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
				case ConditionCode.Equal: compare = context.Operand1.ConstantUnsigned32 == context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.NotEqual: compare = context.Operand1.ConstantUnsigned32 != context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.GreaterOrEqual: compare = context.Operand1.ConstantUnsigned32 >= context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.GreaterThan: compare = context.Operand1.ConstantUnsigned32 > context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.LessOrEqual: compare = context.Operand1.ConstantUnsigned32 <= context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.LessThan: compare = context.Operand1.ConstantUnsigned32 < context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.UnsignedGreaterThan: compare = context.Operand1.ConstantUnsigned32 > context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.UnsignedGreaterOrEqual: compare = context.Operand1.ConstantUnsigned32 >= context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.UnsignedLessThan: compare = context.Operand1.ConstantUnsigned32 < context.Operand2.ConstantUnsigned32; break;
				case ConditionCode.UnsignedLessOrEqual: compare = context.Operand1.ConstantUnsigned32 <= context.Operand2.ConstantUnsigned32; break;
			}

			var e1 = transformContext.CreateConstant(BoolTo64(compare));

			context.SetInstruction(IRInstruction.Move64, context.Result, e1);
		}
	}
}
