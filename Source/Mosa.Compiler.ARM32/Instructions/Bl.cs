// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Instructions;

/// <summary>
/// Bl - Call a subroutine
/// </summary>
public sealed class Bl : ARM32Instruction
{
	internal Bl()
		: base(0, 1)
	{
	}

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		opcodeEncoder.StartOpcode();
		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append4Bits(GetConditionCode(node.ConditionCode));
			opcodeEncoder.Append4Bits(0b1011);
			opcodeEncoder.EmitRelative24(node.Operand1);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
