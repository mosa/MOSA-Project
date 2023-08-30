// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.AddressMode
{
	public abstract class BaseAddressModeTransform : BaseX86Transform
	{
		public BaseAddressModeTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Overrides

		public override bool Match(Context context, TransformContext transform)
		{
			return !IsAddressMode(context);
		}

		#endregion Overrides

		#region Helpers

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

		#endregion Helpers
	}
}
