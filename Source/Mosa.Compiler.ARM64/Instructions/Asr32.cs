// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Instructions;

/// <summary>
/// Asr32 - Arithmetic Shift Right
/// </summary>
public sealed class Asr32 : ARM64Instruction
{
	internal Asr32()
		: base(1, 2)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append4Bits(0b1001);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append6BitImmediate(node.Operand2, 0);
			opcodeEncoder.Append4Bits(0b0111);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append5Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append5Bits(node.Result.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append8Bits(0b11010110);
			opcodeEncoder.Append5Bits(node.Operand2.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b0010);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append2Bits(0b10);
			opcodeEncoder.Append5Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append5Bits(node.Result.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
