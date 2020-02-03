// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic::StorePointer")]
		private static void StorePointer(Context context, MethodCompiler methodCompiler)
		{
			var instruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.Store32 : IRInstruction.Store64;

			if (context.OperandCount == 2)
			{
				context.SetInstruction(instruction, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
			}
			else if (context.OperandCount == 3)
			{
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
