// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// VMov2FP - Move (ARM core register to scalar)
/// </summary>
public sealed class VMov2FP : ARM32Instruction
{
	internal VMov2FP()
		: base(1, 1)
	{
	}

	public override bool IsCommutative => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b1110);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b1011);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append2Bits(0b00);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append4Bits(0b0000);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
