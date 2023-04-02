// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Cpobj Instruction
/// </summary>
/// <seealso cref="BinaryInstruction" />
internal sealed class CpobjInstruction : BinaryInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="CpobjInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public CpobjInstruction(OpCode opcode)
		: base(opcode)
	{
	}

	#endregion Construction
}
