// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// Compare32x32
/// </summary>
public sealed class Compare32x32 : BaseIRInstruction
{
	public Compare32x32()
		: base(2, 1)
	{
	}

	public override bool IsCompare => true;
}
