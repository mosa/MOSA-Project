/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


namespace Mosa.Compiler.Framework.IR
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
