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
	/// Represents an operand stored in a machine register.
	/// </summary>
	public abstract class RegisterOperand : Operand
	{
		
		#region Construction

		/// <summary>
		/// Initializes a new <see cref="DefinedRegisterOperand"/>.
		/// </summary>
		/// <param name="type">The signature type of the value the register holds.</param>
		/// <param name="register">The machine specific register used.</param>
		protected RegisterOperand(SigType type) :
			base(type)
		{
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

	}
}


