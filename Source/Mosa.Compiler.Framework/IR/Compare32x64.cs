// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// Compare32x64
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class Compare32x64 : BaseIRInstruction
{
	public Compare32x64()
		: base(2, 1)
	{
	}

	public override BuiltInType ResultType { get { return BuiltInType.Boolean; } }
}
