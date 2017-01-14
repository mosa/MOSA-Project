// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	public sealed class CompareIntegerBranch : BaseIRInstruction
	{
		#region Construction

		public CompareIntegerBranch()
			: base(0, 2)
		{
		}

		#endregion Construction

		#region IRInstruction Overrides

		public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

		#endregion IRInstruction Overrides
	}
}
