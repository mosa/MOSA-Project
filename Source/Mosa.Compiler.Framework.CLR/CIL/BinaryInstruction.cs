﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Intermediate representation of an instruction, which takes two stack arguments.
/// </summary>
/// <seealso cref="BaseCILInstruction" />
internal class BinaryInstruction : BaseCILInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="BinaryInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public BinaryInstruction(OpCode opcode)
		: base(opcode, 2)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BinaryInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	/// <param name="resultCount">The result count.</param>
	public BinaryInstruction(OpCode opcode, byte resultCount)
		: base(opcode, 2, resultCount)
	{
	}

	#endregion Construction
}
