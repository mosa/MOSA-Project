// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a jump to the global interrupt handler.
	/// </summary>
	internal sealed class GetIDTJumpLocation : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		/// <exception cref="CompilerException"></exception>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var operand = context.Operand1;

			if (!operand.IsResolvedConstant)
			{
				// try to find the constant - a bit of a hack
				Context ctx = new Context(operand.Definitions[0]);

				if (ctx.Instruction == IRInstruction.MoveInteger && ctx.Operand1.IsConstant)
				{
					operand = ctx.Operand1;
				}
			}

			Debug.Assert(operand.IsResolvedConstant);

			int irq = (int)operand.ConstantSignedLongInteger;

			// Find the method
			var method = methodCompiler.TypeSystem.DefaultLinkerType.FindMethodByName("InterruptISR" + irq.ToString());

			if (method == null)
			{
				throw new CompilerException();
			}

			context.SetInstruction(IRInstruction.MoveInteger, context.Result, Operand.CreateSymbolFromMethod(method, methodCompiler.TypeSystem));
		}

		#endregion Methods
	}
}
