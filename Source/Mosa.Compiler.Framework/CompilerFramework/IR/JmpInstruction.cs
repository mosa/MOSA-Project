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
	/// Intermediate representation of an unconditional branch context.
	/// </summary>
	public sealed class JmpInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="JmpInstruction"/> class.
		/// </summary>
		public JmpInstruction()
		{
		}

		#endregion // Construction

		#region IRInstruction Overrides

		public override FlowControl FlowControl
		{
			get
			{
				return FlowControl.Branch;
			}
		}

		/// <summary>
		/// Visits the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.JmpInstruction(context);
		}

		#endregion // IRInstruction Overrides
	}
}
