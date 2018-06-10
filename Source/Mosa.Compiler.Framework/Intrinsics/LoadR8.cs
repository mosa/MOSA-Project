// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	/// <summary>
	/// LoadR8
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IIntrinsicInternalMethod" />
	[ReplacementTarget("Mosa.Runtime.Intrinsic::LoadR8")]
	public sealed class LoadR8 : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <exception cref="CompilerException"></exception>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, MethodCompiler methodCompiler)
		{
			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.LoadFloatR8, context.Result, context.Operand1, methodCompiler.ConstantZero);
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.LoadFloatR8, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderLoadOperands(context.Node, methodCompiler);
		}
	}
}
