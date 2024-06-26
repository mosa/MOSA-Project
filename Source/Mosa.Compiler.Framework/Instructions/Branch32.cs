// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// Branch32
/// </summary>
public sealed class Branch32 : BaseIRInstruction
{
	public Branch32()
		: base(0, 2)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsConditionalBranch => true;

	public override bool IsBranch => true;
}
