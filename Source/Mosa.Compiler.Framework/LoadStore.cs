// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// LoadStore
/// </summary>
public static class LoadStore
{
	public static void Set(Context context, TransformContext transformContext, BaseInstruction instruction, Operand result, Operand operand1, Operand operand2, Operand operand3 = null)
	{
		if (operand1.IsResolvedConstant && operand2.IsResolvedConstant)
		{
			operand1 = transformContext.Is32BitPlatform
				? Operand.CreateConstant32(operand1.ConstantUnsigned32 + operand2.ConstantUnsigned32)
				: Operand.CreateConstant64(operand1.ConstantUnsigned64 + operand2.ConstantUnsigned64);

			operand2 = transformContext.ConstantZero;
		}
		else if (operand1.IsConstant && !operand2.IsConstant)
		{
			var swap = operand1;
			operand1 = operand2;
			operand2 = swap;
		}

		if (transformContext.Is32BitPlatform)
		{
			if (operand1.IsInt64)
			{
				var low = transformContext.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, operand1);
				operand1 = low;
			}

			if (operand2.IsInt64)
			{
				var low = transformContext.VirtualRegisters.Allocate32();
				context.InsertBefore().SetInstruction(IRInstruction.GetLow32, low, operand2);
				operand2 = low;
			}
		}

		if (operand3 == null)
		{
			context.SetInstruction(instruction, result, operand1, operand2);
		}
		else
		{
			context.SetInstruction(instruction, result, operand1, operand2, operand3);
		}
	}
}
