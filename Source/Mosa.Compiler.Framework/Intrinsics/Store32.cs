// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	///
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::Store32")]
	public sealed class Store32 : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <exception cref="CompilerException"></exception>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			const InstructionSize size = InstructionSize.Size32;

			if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.StoreInteger32, size, null, context.Operand1, methodCompiler.ConstantZero, context.Operand2);
			}
			else if (context.OperandCount == 3)
			{
				context.SetInstruction(IRInstruction.StoreInteger32, size, null, context.Operand1, context.Operand2, context.Operand3);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderStoreOperands(context.Node, methodCompiler);
		}
	}
}
