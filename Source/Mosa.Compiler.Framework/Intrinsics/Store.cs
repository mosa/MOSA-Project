// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// Store32
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::Store")]
	public sealed class Store : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <exception cref="CompilerException"></exception>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			if (context.OperandCount == 2)
			{
				var instruction = !context.Operand2.Is64BitInteger ? (BaseInstruction)IRInstruction.StoreInt32 : IRInstruction.StoreInt64;

				context.SetInstruction(instruction, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
			}
			else if (context.OperandCount == 3)
			{
				var instruction = !context.Operand3.Is64BitInteger ? (BaseInstruction)IRInstruction.StoreInt32 : IRInstruction.StoreInt64;

				context.SetInstruction(instruction, null, context.Operand1, context.Operand2, context.Operand3);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderStoreOperands(context.Node, methodCompiler);
		}
	}
}
