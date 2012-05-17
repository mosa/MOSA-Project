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
	/// Intermediate representation of an integer comparison.
	/// </summary>
	public sealed class IntegerCompare : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerCompare"/> class.
		/// </summary>
		public IntegerCompare()
		{
		}

		#endregion // Construction

		#region ThreeOperandInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.IntegerCompare(context);
		}

		#endregion // ThreeOperandInstruction Overrides
	}
}
