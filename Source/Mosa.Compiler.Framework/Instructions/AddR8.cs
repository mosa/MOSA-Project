// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// AddR8
/// </summary>
public sealed class AddR8 : BaseIRInstruction
{
	public AddR8()
		: base(2, 1)
	{
	}

	public override bool IsCommutative => true;
}
