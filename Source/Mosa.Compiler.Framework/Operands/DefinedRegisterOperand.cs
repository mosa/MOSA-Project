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
	public class DefinedRegisterOperand : Operand
	{
		#region Data members

		/// <summary>
		/// The register, where the operand is stored.
		/// </summary>
		private Register register;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new <see cref="DefinedRegisterOperand"/>.
		/// </summary>
		/// <param name="type">The signature type of the value the register holds.</param>
		/// <param name="register">The machine specific register used.</param>
		public DefinedRegisterOperand(SigType type, Register register) :
			base(type)
		{
			if (register == null)
				throw new ArgumentNullException(@"register");

			this.register = register;
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

		/// <summary>
		/// Retrieves the register, where the operand is located.
		/// </summary>
		public Register Register
		{
			get { return register; }
		}

		#endregion // Properties

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="DefinedRegisterOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return String.Format("{0} {1}", register, base.ToString());
		}

		#endregion // Operand Overrides
	}
}


