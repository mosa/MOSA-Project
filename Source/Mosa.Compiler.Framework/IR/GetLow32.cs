// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.IR;

/// <summary>
/// GetLow32
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.IR.BaseIRInstruction" />
public sealed class GetLow32 : BaseIRInstruction
{
	public GetLow32()
		: base(1, 1)
	{
	}

	public override BuiltInType ResultType { get { return BuiltInType.UInt32; } }

	public override BuiltInType ResultType2 { get { return BuiltInType.UInt32; } }
}
