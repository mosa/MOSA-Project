// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Internal.Intrinsic::Load32")]
	public sealed class Load32 : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var size = InstructionSize.Size32;

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
