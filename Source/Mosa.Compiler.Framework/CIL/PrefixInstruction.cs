// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL;

/// <summary>
/// Prefix Instruction
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
public class PrefixInstruction : BaseCILInstruction
{
	#region Properties

	/// <summary>
	/// Gets the prefix flag.
	/// </summary>
	/// <value>A prefix fla</value>
	public Prefix Flags
	{
		get
		{
			switch (OpCode)
			{
				case OpCode.Constrained: return Prefix.Constrained;
				case OpCode.No: return Prefix.No;
				case OpCode.ReadOnly: return Prefix.ReadOnly;
				case OpCode.Tailcall: return Prefix.Tail;
				case OpCode.Unaligned: return Prefix.Unaligned;
				case OpCode.Volatile: return Prefix.Volatile;
				default:
					throw new InvalidOperationException("Unknown prefix instruction codeReader.");
			}
		}
	}

	#endregion Properties

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="PrefixInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	protected PrefixInstruction(OpCode opcode)
		: base(opcode, 0)
	{
	}

	#endregion Construction
}
