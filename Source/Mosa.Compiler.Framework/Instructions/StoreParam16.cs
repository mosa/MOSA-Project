// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// StoreParam16
/// </summary>
public sealed class StoreParam16 : BaseIRInstruction
{
	public StoreParam16()
		: base(2, 0)
	{
	}

	public override bool IsMemoryWrite => true;

	public override bool IsParameterStore => true;
}
