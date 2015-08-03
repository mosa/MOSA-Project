// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of an unconditional branch context.
	/// </summary>
	public sealed class Jmp : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Jmp"/> class.
		/// </summary>
		public Jmp()
			: base(1, 0)
		{
		}

		#endregion Construction

		#region IRInstruction Overrides

		public override FlowControl FlowControl { get { return FlowControl.UnconditionalBranch; } }

		/// <summary>
		/// Visits the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.Jmp(context);
		}

		#endregion IRInstruction Overrides
	}
}