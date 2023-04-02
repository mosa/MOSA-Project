// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Mul Instruction
/// </summary>
/// <seealso cref="ArithmeticInstruction" />
internal sealed class MulInstruction : ArithmeticInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="MulInstruction"/> class.
	/// </summary>
	/// <param name="opCode">The op code.</param>
	public MulInstruction(OpCode opCode)
		: base(opCode)
	{
	}

	#endregion Construction
}
