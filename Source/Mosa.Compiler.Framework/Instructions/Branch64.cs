// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// Branch64
/// </summary>
public sealed class Branch64 : BaseIRInstruction
{
	public Branch64()
		: base(0, 2)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsConditionalBranch => true;

	public override bool IsBranch => true;
}
