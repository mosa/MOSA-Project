/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR2
{
	/// <summary>
	/// Retrieves the address of the variable represented by its operand.
	/// </summary>
	/// <remarks>
	/// The address of instruction is used to retrieve the memory address
	/// of its sole operand. The operand may not represent a register.
	/// </remarks>
	public sealed class AddressOfInstruction : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddressOfInstruction"/>.
		/// </summary>
		public AddressOfInstruction()
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Returns a string representation of the instruction.
		/// </summary>
		/// <returns>
		/// A string representation of the instruction in intermediate form.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("IR {0} = &{1}", instruction.Operand1, instruction.Operand2);
		}

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.AddressOfInstruction(context);
		}

		#endregion // Methods
	}
}
