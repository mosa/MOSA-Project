// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public sealed class IntegerCompareBranch : BaseIRInstruction
	{
		#region Construction

		public IntegerCompareBranch()
			: base(0, 2)
		{
		}

		#endregion Construction

		#region IRInstruction Overrides

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.IntegerCompareBranch(context);
		}

		#endregion IRInstruction Overrides
	}
}
