/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  
 */


namespace Mosa.Runtime.CompilerFramework.IR
{
	/// <summary>
	/// Represents a floating point comparison context.
	/// </summary>
	public sealed class FloatingPointCompareInstruction : ThreeOperandInstruction
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatingPointCompareInstruction"/> class.
		/// </summary>
		public FloatingPointCompareInstruction()
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
			visitor.FloatingPointCompareInstruction(context);
		}

		#endregion // ThreeOperandInstruction Overrides
	}
}
