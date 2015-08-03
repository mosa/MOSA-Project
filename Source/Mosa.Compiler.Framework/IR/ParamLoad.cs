// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Loads a value passed on the stack.
	/// </summary>
	/// <remarks>
	/// The param load instruction is used to load a value passed the stack. The types must be compatible.
	/// </remarks>
	public sealed class ParamLoad : ThreeOperandInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Load"/>.
		/// </summary>
		public ParamLoad()
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
			//visitor.ParamLoad(context);
		}

		#endregion TwoOperandInstruction Overrides
	}
}