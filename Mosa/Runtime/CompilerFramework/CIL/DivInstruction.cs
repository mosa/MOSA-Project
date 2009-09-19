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
	public class DivInstruction : ArithmeticInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DivInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public DivInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{0} ; {1} = {2} / {3}", base.ToString(), instruction.Result, instruction.Operand1, instruction.Operand2);
		}

		#endregion Methods

	}
}
