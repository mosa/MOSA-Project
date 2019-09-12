// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic:Store")]
		private static void Store(Context context, MethodCompiler methodCompiler)
		{
			if (context.OperandCount == 2)
			{
				var instruction = !context.Operand2.Is64BitInteger ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;

				if (context.Operand2.IsR4)
					instruction = IRInstruction.StoreR4;
				else if (context.Operand2.IsR8)
					instruction = IRInstruction.StoreR8;

				context.SetInstruction(instruction, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
			}
			else if (context.OperandCount == 3)
			{
				var instruction = !context.Operand3.Is64BitInteger ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;

				if (context.Operand3.IsR4)
					instruction = IRInstruction.StoreR4;
				else if (context.Operand3.IsR8)
					instruction = IRInstruction.StoreR8;

				context.SetInstruction(instruction, null, context.Operand1, context.Operand2, context.Operand3);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderStoreOperands(context, methodCompiler);
		}
	}
}
