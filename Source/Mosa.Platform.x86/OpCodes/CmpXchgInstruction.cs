/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */


using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.OpCodes
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
