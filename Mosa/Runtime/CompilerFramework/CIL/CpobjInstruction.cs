/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class CpobjInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CpobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CpobjInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region CILInstruction Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{2} ; *{0} = *{1}", instruction.Operand1, instruction.Operand2, base.ToString());
		}

		#endregion // CILInstruction Overrides

	}
}
