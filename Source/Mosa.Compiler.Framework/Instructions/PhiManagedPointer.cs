// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Instructions;

/// <summary>
/// PhiManagedPointer
/// </summary>
public sealed class PhiManagedPointer : BaseIRInstruction
{
	public PhiManagedPointer()
		: base(0, 0)
	{
	}

	public override bool IsPhi => true;

	public override bool HasVariableOperands => true;
}
