// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// StableObjectTracking
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class StableObjectTracking : BaseIRInstruction
{
	public StableObjectTracking()
		: base(0, 0)
	{
	}

	public override bool IgnoreDuringCodeGeneration { get { return true; } }
}
