// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions;

/// <summary>
/// Out16
/// </summary>
/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
public sealed class Out16 : X86Instruction
{
	internal Out16()
		: base(0, 2)
	{
	}

	public override bool IsIOOperation { get { return true; } }

	public override bool HasUnspecifiedSideEffect { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		if (node.Operand1.IsCPURegister)
		{
			opcodeEncoder.Append8Bits(0xEF);
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xE7);
			opcodeEncoder.Append8BitImmediate(node.Operand1);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
