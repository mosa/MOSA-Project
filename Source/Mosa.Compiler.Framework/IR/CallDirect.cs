// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// CallDirect
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
	public sealed class CallDirect : BaseIRInstruction
	{
		public CallDirect()
			: base(0, 0)
		{
		}

		public override FlowControl FlowControl { get { return FlowControl.Call; } }

		public override bool VariableOperands { get { return true; } }
	}
}
