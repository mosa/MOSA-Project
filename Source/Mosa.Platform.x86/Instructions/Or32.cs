// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions;

/// <summary>
/// Or32
/// </summary>
/// <seealso cref="Mosa.Platform.x86.X86Instruction" />
public sealed class Or32 : X86Instruction
{
	internal Or32()
		: base(1, 2)
	{
	}

	public override bool IsCommutative { get { return true; } }

	public override bool IsZeroFlagModified { get { return true; } }

	public override bool IsCarryFlagCleared { get { return true; } }

	public override bool IsCarryFlagModified { get { return true; } }

	public override bool IsSignFlagModified { get { return true; } }

	public override bool IsOverflowFlagCleared { get { return true; } }

	public override bool IsOverflowFlagModified { get { return true; } }

	public override bool IsParityFlagModified { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsCPURegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 0) && (node.Operand2.IsConstant && node.Operand2.ConstantSigned32 >= -128 && node.Operand2.ConstantSigned32 <= 127))
		{
			opcodeEncoder.Append8Bits(0x83);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b001);
			opcodeEncoder.Append3Bits(0b000);
			opcodeEncoder.Append8BitImmediate(node.Operand2);
			return;
		}

		if ((node.Operand1.IsCPURegister && node.Operand1.Register.RegisterCode == 0) && node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x0D);
			opcodeEncoder.Append32BitImmediate(node.Operand2);
			return;
		}

		if (node.Operand2.IsCPURegister)
		{
			opcodeEncoder.Append8Bits(0x0B);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		if (node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x81);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b001);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append32BitImmediate(node.Operand2);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
