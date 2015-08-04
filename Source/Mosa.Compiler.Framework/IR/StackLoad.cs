// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Loads a value from the stack.
	/// </summary>
	/// <remarks>
	/// The stack load instruction is used to load a value from the stack. The types must be compatible.
	/// </remarks>
	public sealed class StackLoad : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Load"/>.
		/// </summary>
		public StackLoad()
		{
		}

		#endregion Construction

		#region TwoOperandInstruction Overrides

		/// <summary>
		/// Visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			//visitor.StackLoad(context);
		}

		#endregion TwoOperandInstruction Overrides
	}
}
