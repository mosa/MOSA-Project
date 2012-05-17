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
	/// An abstract intermediate representation of the method prologue.
	/// </summary>
	/// <remarks>
	/// This instruction is usually derived by the architecture and expanded appropriately
	/// for the calling convention of the method.
	/// </remarks>
	public sealed class Prologue : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Prologue"/>.
		/// </summary>
		public Prologue() :
			base(0, 0)
		{
		}

		#endregion // Construction

		#region Instruction Overrides

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.Prologue(context);
		}

		#endregion // Instruction Overrides
	}
}
