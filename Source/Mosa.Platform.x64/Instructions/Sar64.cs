// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Sar64
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Sar64 : X64Instruction
{
	internal Sar64()
		: base(1, 2)
	{
	}

	public override bool IsZeroFlagModified { get { return true; } }

	public override bool IsCarryFlagModified { get { return true; } }

	public override bool IsSignFlagModified { get { return true; } }

	public override bool IsOverflowFlagModified { get { return true; } }

	public override bool IsParityFlagModified { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		if (node.Operand2.IsCPURegister)
		{
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0xD3);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b111);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			return;
		}

		if (node.Operand2.IsConstantOne)
		{
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0xD1);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b111);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			return;
		}

		if (node.Operand2.IsConstant)
		{
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(node.Result.Register.RegisterCode >> 3);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append8Bits(0xC1);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b111);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
