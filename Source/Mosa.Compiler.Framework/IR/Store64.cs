// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// Store64
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class Store64 : BaseIRInstruction
{
	public Store64()
		: base(3, 0)
	{
	}

	public override bool IsMemoryWrite { get { return true; } }
}
