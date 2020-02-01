// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Runtime.Intrinsic::Load16")]
		private static void Load16(Context context, MethodCompiler methodCompiler)
		{
			var instruction = methodCompiler.Architecture.Is32BitPlatform ? (BaseInstruction)IRInstruction.LoadZeroExtend16x32 : IRInstruction.LoadZeroExtend16x64;

			if (context.OperandCount == 1)
			{
				context.SetInstruction(instruction, context.Result, context.Operand1, methodCompiler.ConstantZero);
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(instruction, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderLoadOperands(context, methodCompiler);
		}
	}
}
