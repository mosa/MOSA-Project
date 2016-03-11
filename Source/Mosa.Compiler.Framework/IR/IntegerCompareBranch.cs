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

		#endregion IRInstruction Overrides
	}
}
