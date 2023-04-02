// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Rem Instruction
/// </summary>
/// <seealso cref="ArithmeticInstruction" />
internal sealed class RemInstruction : ArithmeticInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="RemInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public RemInstruction(OpCode opcode)
		: base(opcode)
	{
	}

	#endregion Construction
}
