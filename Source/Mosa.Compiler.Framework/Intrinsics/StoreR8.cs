// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic:StoreR8")]
		private static void StoreR8(Context context, MethodCompiler methodCompiler)
		{
			if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.StoreR8, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
			}
			else if (context.OperandCount == 3)
			{
				context.SetInstruction(IRInstruction.StoreR8, null, context.Operand1, context.Operand2, context.Operand3);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderStoreOperands(context, methodCompiler);
		}
	}
}
