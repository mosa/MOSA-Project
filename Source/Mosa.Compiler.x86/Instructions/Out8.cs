// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Out8
/// </summary>
public sealed class Out8 : X86Instruction
{
	internal Out8()
		: base(0, 2)
	{
	}

	public override bool IsIOOperation => true;

	public override bool HasUnspecifiedSideEffect => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 0);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsPhysicalRegister)
		{
			opcodeEncoder.Append8Bits(0xEE);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append8Bits(0xE6);
			opcodeEncoder.AppendInteger8(node.Operand1);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
