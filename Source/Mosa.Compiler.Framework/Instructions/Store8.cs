// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// Store8
/// </summary>
public sealed class Store8 : BaseIRInstruction
{
	public Store8()
		: base(3, 0)
	{
	}

	public override bool IsMemoryWrite => true;
}
