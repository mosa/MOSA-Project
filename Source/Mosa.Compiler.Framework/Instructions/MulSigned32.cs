// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// MulSigned32
/// </summary>
public sealed class MulSigned32 : BaseIRInstruction
{
	public MulSigned32()
		: base(2, 1)
	{
	}

	public override bool IsCommutative => true;
}