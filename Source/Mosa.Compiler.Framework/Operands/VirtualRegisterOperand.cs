/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Operands
{
	/// <summary>
	/// Represents an operand stored in a machine specific register.
	/// </summary>
	public class VirtualRegisterOperand : Operand
	{
		#region Data members

		private int index;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualRegisterOperand"/> class.
		/// </summary>
		/// <param name="type">The signature type of the value the register holds.</param>
		/// <param name="index">The index.</param>
		public VirtualRegisterOperand(SigType type, int index) :
			base(type)
		{
			this.index = index;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// This is a register operand.
		/// </summary>
		public override bool IsRegister
		{
			get { return true; }
		}

		#endregion // Properties

		#region Operand Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A string representation of the operand.
		/// </returns>
		public override string ToString()
		{
			return String.Format("V_{0} {1}", index, base.ToString());
		}

		#endregion // Operand Overrides
	}
}


