// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64
{
	public static class X64TransformHelper
	{
		public static Operand MoveConstantToFloatRegister(TransformContext transform, Context context, Operand operand)
		{
			if (!operand.IsConstant)
				return operand;

			var label = transform.CreateFloatingPointLabel(operand);

			var v1 = operand.IsR4 ? transform.AllocateVirtualRegisterR4() : transform.AllocateVirtualRegisterR8();

			var instruction = operand.IsR4 ? (BaseInstruction)X64.MovssLoad : X64.MovsdLoad;

			context.InsertBefore().SetInstruction(instruction, v1, label, transform.Constant32_0);

			return v1;
		}

		public static void AddressModeConversion(Context context, BaseInstruction instruction)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			context.InsertBefore().SetInstruction(instruction, result, operand1);
			context.Operand1 = result;
		}

		public static bool IsAddressMode(Context context)
		{
			if (context.Result == context.Operand1)
				return true;

			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register)
				return true;

			return false;
		}

		public static void AddressModeConversionCummulative(Context context, BaseInstruction instruction)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (result == operand2 && result != operand1)
			{
				context.Operand2 = operand1;
				context.Operand1 = operand2;
			}
			else
			{
				context.InsertBefore().SetInstruction(instruction, result, operand1);
				context.Operand1 = result;
			}
		}
	}
}
