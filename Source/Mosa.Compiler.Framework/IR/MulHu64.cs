// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// MulHu64 - Multiply high unsigned (future)
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class MulHu64 : BaseIRInstruction
{
	public MulHu64()
		: base(2, 1)
	{
	}

	public override bool IsCommutative => true;
}
