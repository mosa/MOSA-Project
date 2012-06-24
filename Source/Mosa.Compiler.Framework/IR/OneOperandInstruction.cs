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
	/// Abstract base class for IR instructions with one operand.
	/// </summary>
	/// <remarks>
	/// The <see cref="OneOperandInstruction"/> is the base class for
	/// IR instructions using one operand.
	/// </remarks>
	public abstract class OneOperandInstruction : BaseIRInstruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="ThreeOperandInstruction"/>.
		/// </summary>
		public OneOperandInstruction() :
			base(1, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="TwoOperandInstruction"/>.
		/// </summary>
		/// <param name="op">The unary operand of this instruction.</param>
		public OneOperandInstruction(Operand op) :
			base(1, 0)
		{
		}

		#endregion // Construction

	}
}
