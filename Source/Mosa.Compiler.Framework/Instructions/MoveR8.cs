// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// MoveR8
/// </summary>
public sealed class MoveR8 : BaseIRInstruction
{
	public MoveR8()
		: base(1, 1)
	{
	}

	public override bool IsMove => true;
}
