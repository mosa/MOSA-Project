// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// SetReturn64
/// </summary>
public sealed class SetReturn64 : BaseIRInstruction
{
	public SetReturn64()
		: base(1, 0)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsReturn => true;
}
