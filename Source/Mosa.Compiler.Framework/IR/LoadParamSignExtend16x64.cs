// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// LoadParamSignExtend16x64
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class LoadParamSignExtend16x64 : BaseIRInstruction
{
	public LoadParamSignExtend16x64()
		: base(1, 1)
	{
	}

	public override bool IsMemoryRead { get { return true; } }

	public override bool IsParameterLoad { get { return true; } }
}
