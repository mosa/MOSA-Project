/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Internal.Intrinsic::Load64")]
	public sealed class Load64 : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var size = InstructionSize.Size64;

			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.Load, size, context.Result, context.Operand1, Operand.CreateConstant(methodCompiler.TypeSystem, 0));
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.Load, size, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}
	}
}
