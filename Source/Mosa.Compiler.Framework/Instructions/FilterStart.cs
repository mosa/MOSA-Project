// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// FilterStart
/// </summary>
public sealed class FilterStart : BaseIRInstruction
{
	public FilterStart()
		: base(0, 1)
	{
	}

	public override bool IgnoreDuringCodeGeneration => true;
}