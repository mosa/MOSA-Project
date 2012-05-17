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
	/// Intermediate representation of a floating point to integral conversion operation.
	/// </summary>
	public sealed class FloatToIntegerConversion : TwoOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatToIntegerConversion"/> class.
		/// </summary>
		public FloatToIntegerConversion()
		{
		}

		#endregion // Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.FloatToIntegerConversion(context);
		}

		#endregion // TwoOperandInstruction Overrides
	}
}
