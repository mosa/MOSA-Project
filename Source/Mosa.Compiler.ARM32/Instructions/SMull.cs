// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// SMull - Multiply Signed Long
/// </summary>
public sealed class SMull : ARM32Instruction
{
	internal SMull()
		: base(2, 2)
	{
	}

	public override bool IsZeroFlagModified => true;

	public override bool IsCarryFlagModified => true;

	public override bool IsCarryFlagUnchanged => true;

	public override bool IsCarryFlagUndefined => true;

	public override bool IsSignFlagModified => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 2);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		if (node.Operand1.IsPhysicalRegister && node.Operand2.IsPhysicalRegister)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b0000);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b1);
			opcodeEncoder.Append1Bit(0b0);
			opcodeEncoder.Append1Bit(node.IsSetFlags ? 1 : 0);
			opcodeEncoder.Append4Bits(node.Result.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Result2.Register.RegisterCode);
			opcodeEncoder.Append4Bits(node.Operand1.Register.RegisterCode);
			opcodeEncoder.Append4Bits(0b1001);
			opcodeEncoder.Append4Bits(node.Operand2.Register.RegisterCode);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
