// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Btr32
/// </summary>
public sealed class Btr32 : X86Instruction
{
	internal Btr32()
		: base(1, 2)
	{
	}

	public override bool IsCarryFlagModified => true;

	public override bool IsSignFlagUnchanged => true;

	public override bool IsSignFlagUndefined => true;

	public override bool IsOverflowFlagUnchanged => true;

	public override bool IsOverflowFlagUndefined => true;

	public override bool IsParityFlagUnchanged => true;

	public override bool IsParityFlagUndefined => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(node.Result.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0xBA);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append3Bits(node.Operand2.Register.RegisterCode);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand2.IsConstant)
		{
			opcodeEncoder.Append8Bits(0x0F);
			opcodeEncoder.Append8Bits(0xB3);
			opcodeEncoder.Append2Bits(0b11);
			opcodeEncoder.Append3Bits(0b110);
			opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.AppendInteger8(node.Operand2);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
