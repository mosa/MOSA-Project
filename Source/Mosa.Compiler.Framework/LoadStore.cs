// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// LoadStore
	/// </summary>
	public static class LoadStore
	{
		public static void OrderLoadOperands(InstructionNode node, MethodCompiler methodCompiler)
		{
			if (node.Operand1.IsResolvedConstant && node.Operand2.IsResolvedConstant)
			{
				node.Operand1 = methodCompiler.CreateConstant(node.Operand1.ConstantUnsignedLongInteger + node.Operand2.ConstantUnsignedLongInteger);
				node.Operand2 = methodCompiler.ConstantZero;
			}

			if (node.Operand1.IsConstant && !node.Operand2.IsConstant)
			{
				var operand1 = node.Operand1;
				var operand2 = node.Operand2;

				node.Operand2 = operand1;
				node.Operand1 = operand2;
			}
		}

		public static void OrderStoreOperands(InstructionNode node, MethodCompiler methodCompiler)
		{
			if (node.Operand1.IsResolvedConstant && node.Operand2.IsResolvedConstant)
			{
				node.Operand1 = methodCompiler.CreateConstant(node.Operand1.ConstantUnsignedLongInteger + node.Operand2.ConstantUnsignedLongInteger);
				node.Operand2 = methodCompiler.ConstantZero;
			}

			if (node.Operand1.IsConstant && !node.Operand2.IsConstant)
			{
				var operand1 = node.Operand1;
				var operand2 = node.Operand2;

				node.Operand2 = operand1;
				node.Operand1 = operand2;
			}
		}
	}
}
