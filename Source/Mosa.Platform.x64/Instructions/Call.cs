// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Call
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Call : X64Instruction
{
	internal Call()
		: base(0, 1)
	{
	}

	public override FlowControl FlowControl { get { return FlowControl.Call; } }

	public override bool IsZeroFlagUnchanged { get { return true; } }

	public override bool IsZeroFlagUndefined { get { return true; } }

	public override bool IsCarryFlagUnchanged { get { return true; } }

	public override bool IsCarryFlagUndefined { get { return true; } }

	public override bool IsSignFlagUnchanged { get { return true; } }

	public override bool IsSignFlagUndefined { get { return true; } }

	public override bool IsOverflowFlagUnchanged { get { return true; } }

	public override bool IsOverflowFlagUndefined { get { return true; } }

	public override bool IsParityFlagUnchanged { get { return true; } }

	public override bool IsParityFlagUndefined { get { return true; } }

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		if (node.Operand1.IsCPURegister)
		{
			opcodeEncoder.SuppressByte(0x40);
			opcodeEncoder.Append4Bits(0b0100);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.Operand1.Register.RegisterCode >> 3);
			opcodeEncoder.Append8Bits(0xFF);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b010);
			opcodeEncoder.Append3Bits(node.Operand1.Register.RegisterCode);
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xE8);
			opcodeEncoder.EmitRelative32(node.Operand1);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
