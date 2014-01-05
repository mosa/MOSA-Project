/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System;
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
				Context def = new Context(context.InstructionSet, operand.Definitions[0]);

				if (def.Instruction is Move && def.Operand1.IsConstant)
					operand = def.Operand1;
			}

			Debug.Assert(operand.IsConstant);

			int irq = (int)operand.ConstantSignedInteger;

			if ((irq > 256) || (irq < 0))
				throw new InvalidOperationException();

			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateUnmanagedSymbolPointer(methodCompiler.TypeSystem, "Mosa.Tools.Compiler.LinkerGenerated.<$>InterruptISR" + irq.ToString() + "()"));
		}

		#endregion Methods
	}
}