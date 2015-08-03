// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Retrieves the address of the variable represented by its operand.
	/// </summary>
	/// <remarks>
	/// The address of instruction is used to retrieve the memory address
	/// of its sole operand. The operand may not represent a register.
	/// </remarks>
	public sealed class AddressOf : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AddressOf"/>.
		/// </summary>
		public AddressOf()
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.AddressOf(context);
		}

		#endregion Methods
	}
}