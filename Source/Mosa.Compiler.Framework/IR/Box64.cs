// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// Box64
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class Box64 : BaseIRInstruction
{
	public Box64()
		: base(2, 1)
	{
	}

	public override bool IsMemoryWrite { get { return true; } }

	public override bool IsMemoryRead { get { return true; } }
}
