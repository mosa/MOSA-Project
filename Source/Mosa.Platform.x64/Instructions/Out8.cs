// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Instructions;

/// <summary>
/// Out8
/// </summary>
/// <seealso cref="Mosa.Platform.x64.X64Instruction" />
public sealed class Out8 : X64Instruction
{
	internal Out8()
		: base(0, 2)
	{
	}

	public override bool IsIOOperation => true;

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(InstructionNode node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);

		if (node.Operand1.IsCPURegister)
		{
			opcodeEncoder.Append8Bits(0xEE);
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xE6);
			opcodeEncoder.Append8BitImmediate(node.Operand1);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
