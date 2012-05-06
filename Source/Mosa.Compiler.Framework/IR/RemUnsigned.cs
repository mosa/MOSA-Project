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
	/// Intermediate representation of the unsigned remainder operation.
	/// </summary>
	public sealed class RemUnsigned : ThreeOperandInstruction
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RemUnsigned"/> class.
		/// </summary>
		public RemUnsigned()
		{
		}

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.RemUnsigned(context);
		}

	}
}
