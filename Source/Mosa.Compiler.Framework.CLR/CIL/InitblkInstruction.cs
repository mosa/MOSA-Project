﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Initblk Instruction
/// </summary>
/// <seealso cref="NaryInstruction" />
internal sealed class InitblkInstruction : NaryInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="InitblkInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public InitblkInstruction(OpCode opcode)
		: base(opcode, 3)
	{
	}

	#endregion Construction
}
