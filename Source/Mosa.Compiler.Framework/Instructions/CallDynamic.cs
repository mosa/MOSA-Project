// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// CallDynamic - The instruction represents a method called where method address is provide by a virtual register
/// </summary>
public sealed class CallDynamic : BaseIRInstruction
{
	public CallDynamic()
		: base(0, 0)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsCall => true;

	public override bool HasVariableOperands => true;
}
