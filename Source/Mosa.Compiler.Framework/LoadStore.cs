// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// LoadStore
	/// </summary>
	public static class LoadStore
	{
		public static void OrderLoadOperands(Context context, MethodCompiler methodCompiler)
		{
			if (context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant)
			{
				context.Operand1 = methodCompiler.CreateConstant(context.Operand1.ConstantUnsignedLongInteger + context.Operand2.ConstantUnsignedLongInteger);
				context.Operand2 = methodCompiler.ConstantZero;
			}

			if (context.Operand1.IsConstant && !context.Operand2.IsConstant)
			{
				var operand1 = context.Operand1;
				var operand2 = context.Operand2;

				context.Operand2 = operand1;
				context.Operand1 = operand2;
			}
		}

		public static void OrderStoreOperands(Context context, MethodCompiler methodCompiler)
		{
			if (context.Operand1.IsResolvedConstant && context.Operand2.IsResolvedConstant)
			{
				context.Operand1 = methodCompiler.CreateConstant(context.Operand1.ConstantUnsignedLongInteger + context.Operand2.ConstantUnsignedLongInteger);
				context.Operand2 = methodCompiler.ConstantZero;
			}

			if (context.Operand1.IsConstant && !context.Operand2.IsConstant)
			{
				var operand1 = context.Operand1;
				var operand2 = context.Operand2;

				context.Operand2 = operand1;
				context.Operand1 = operand2;
			}
		}
	}
}
