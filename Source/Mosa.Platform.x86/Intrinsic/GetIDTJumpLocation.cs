/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a jump to the global interrupt handler.
	/// </summary>
	public sealed class GetIDTJumpLocation : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var operand = context.Operand1;

			if (!operand.IsConstant)
			{
				// try to find the constant - a bit of a hack
				Context ctx = new Context(context.InstructionSet, operand.Definitions[0]);

				if (ctx.Instruction == IRInstruction.Move && ctx.Operand1.IsConstant)
				{
					operand = ctx.Operand1;
				}
			}

			Debug.Assert(operand.IsConstant);

			int irq = (int)operand.ConstantSignedInteger;

			// Find the method
			var method = methodCompiler.TypeSystem.DefaultLinkerType.FindMethodByName("InterruptISR" + irq.ToString());

			if (method == null)
			{
				throw new InvalidCompilerException();
			}

			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateSymbolFromMethod(methodCompiler.TypeSystem, method));
		}

		#endregion Methods
	}
}