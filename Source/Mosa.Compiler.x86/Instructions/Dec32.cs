// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Instructions;

/// <summary>
/// Dec32
/// </summary>
public sealed class Dec32 : X86Instruction
{
	internal Dec32()
		: base(1, 1)
	{
	}

	public override bool IsZeroFlagModified => true;

	public override bool IsSignFlagModified => true;

	public override bool IsOverflowFlagModified => true;

	public override bool IsParityFlagModified => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 1);
		System.Diagnostics.Debug.Assert(node.Result.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Operand1.IsPhysicalRegister);
		System.Diagnostics.Debug.Assert(node.Result.Register == node.Operand1.Register);

		opcodeEncoder.Append4Bits(0b0100);
		opcodeEncoder.Append1Bit(0b1);
		opcodeEncoder.Append3Bits(node.Result.Register.RegisterCode);
	}
}
