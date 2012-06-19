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
	/// Represents an operand, that is located on the relative to the current stack frame.
	/// </summary>
	public abstract class StackOperand : MemoryOperand
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="StackOperand"/>.
		/// </summary>
		/// <param name="register">Holds the stack frame register.</param>
		/// <param name="type">Holds the type of data held in this operand.</param>
		/// <param name="offset">The offset of the variable on stack. A positive value reflects the current function stack, a negative offset indicates a parameter.</param>
		protected StackOperand(Register register, SigType type, int offset) :
			base(register, type, new IntPtr(offset * 4))
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the name of the stack operand.
		/// </summary>
		/// <value>The name of the stack operand.</value>
		public abstract string Name { get; }

		#endregion // Properties

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public sealed override string ToString()
		{
			return String.Format(@"{0} {1}", Name, base.ToString());
		}

		#endregion // Operand Overrides

	}
}


