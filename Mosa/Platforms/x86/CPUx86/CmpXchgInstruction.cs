/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 compare-exchange instruction.
	/// </summary>
	/// <remarks>
	/// This instruction compares the value of Operand0 and Operand1. If they are
	/// equal, Operand0 is set to the value of Operand2.
	/// </remarks>
	public sealed class CmpXchgInstruction : ThreeOperandInstruction
	{

		#region Methods

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(Context context)
		{
			return String.Format(@"X86.cmpxchg {0}, {1}, {2} ; if ({0} == {1}) {0} = {2} ", context.Operand1, context.Operand2, context.Operand2);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.CmpXchg(context);
		}

		#endregion // Methods
	}
}
