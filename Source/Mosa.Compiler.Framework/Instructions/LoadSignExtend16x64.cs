// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// LoadSignExtend16x64
/// </summary>
public sealed class LoadSignExtend16x64 : BaseIRInstruction
{
	public LoadSignExtend16x64()
		: base(2, 1)
	{
	}

	public override bool IsMemoryRead => true;
}
