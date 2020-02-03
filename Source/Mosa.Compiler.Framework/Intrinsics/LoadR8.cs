// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic::LoadR8")]
		private static void LoadR8(Context context, MethodCompiler methodCompiler)
		{
			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.LoadR8, context.Result, context.Operand1, methodCompiler.ConstantZero);
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.LoadR8, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderLoadOperands(context, methodCompiler);
		}
	}
}
