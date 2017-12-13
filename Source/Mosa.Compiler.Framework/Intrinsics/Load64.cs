// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
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
		/// <exception cref="CompilerException"></exception>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			const InstructionSize size = InstructionSize.Size64;

			BaseIRInstruction instruction = IRInstruction.LoadZeroExtended;

			if (methodCompiler.Architecture.NativeIntegerSize == 64)
				instruction = IRInstruction.LoadInteger;

			if (context.OperandCount == 1)
			{
				context.SetInstruction(IRInstruction.LoadZeroExtended, size, context.Result, context.Operand1, methodCompiler.ConstantZero);
			}
			else if (context.OperandCount == 2)
			{
				context.SetInstruction(IRInstruction.LoadZeroExtended, size, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				throw new CompilerException();
			}

			LoadStore.OrderLoadOperands(context.Node, methodCompiler);
		}
	}
}
