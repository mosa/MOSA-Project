// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// Load32
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class Load32 : BaseIRInstruction
{
	public Load32()
		: base(2, 1)
	{
	}

	public override bool IsMemoryRead { get { return true; } }
}
