// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// In8
/// </summary>
/// <seealso cref="Mosa.Compiler.x86.X86Instruction" />
public sealed class In8 : X86Instruction
{
	internal In8()
		: base(1, 1)
	{
	}

	public override bool IsIOOperation => true;

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);

		if (node.Operand1.IsCPURegister)
		{
			opcodeEncoder.Append8Bits(0xEC);
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xE4);
			opcodeEncoder.Append8BitImmediate(node.Operand1);
			return;
		}

		throw new Compiler.Common.Exceptions.CompilerException("Invalid Opcode");
	}
}
