// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// AddCarryOut64
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class AddCarryOut64 : BaseIRInstruction
{
	public AddCarryOut64()
		: base(2, 2)
	{
	}

	public override bool IsCommutative { get { return true; } }
}
