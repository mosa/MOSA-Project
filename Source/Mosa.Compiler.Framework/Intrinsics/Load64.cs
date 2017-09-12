// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Runtime.Intrinsic::Load64")]
	public sealed class Load64 : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			const InstructionSize size = InstructionSize.Size64;

			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.LoadInteger, size, context.Result, context.Operand1, Operand.CreateConstant(0, methodCompiler.TypeSystem));
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.LoadInteger, size, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new InvalidCompilerException();
			}
		}
	}
}
