// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// CompareR8
/// </summary>
public sealed class CompareR8 : BaseIRInstruction
{
	public CompareR8()
		: base(2, 1)
	{
	}

	public override bool IsCompare => true;
}
