// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Instructions;

/// <summary>
/// In16
/// </summary>
public sealed class In16 : X64Instruction
{
	internal In16()
		: base(1, 1)
	{
	}

	public override bool IsIOOperation => true;

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x66);
			opcodeEncoder.Append8Bits(0xED);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x66);
			opcodeEncoder.Append8Bits(0xE4);
			opcodeEncoder.Append8BitImmediate(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
