﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Nop Instruction
/// </summary>
/// <seealso cref="BaseCILInstruction" />
internal sealed class NopInstruction : BaseCILInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="NopInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public NopInstruction(OpCode opcode)
		: base(opcode, 0)
	{
	}

	#endregion Construction
}
