// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// StableObjectTracking
/// </summary>
public sealed class StableObjectTracking : BaseIRInstruction
{
	public StableObjectTracking()
		: base(0, 0)
	{
	}

	public override bool IgnoreDuringCodeGeneration => true;
}
