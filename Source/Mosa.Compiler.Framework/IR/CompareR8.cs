// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// CompareR8
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class CompareR8 : BaseIRInstruction
{
	public CompareR8()
		: base(2, 1)
	{
	}

	public override BuiltInType ResultType { get { return BuiltInType.Boolean; } }
}
