// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Instructions;

/// <summary>
/// B - Unconditional branch
/// </summary>
public sealed class B : ARM64Instruction
{
	internal B()
		: base(1, 2)
	{
	}

	public override bool IsFlowNext => false;

	public override bool IsConditionalBranch => true;

	public override bool IsCommutative => true;

	public override void Emit(Node node, OpcodeEncoder opcodeEncoder)
	{
		System.Diagnostics.Debug.Assert(node.ResultCount == 1);
		System.Diagnostics.Debug.Assert(node.OperandCount == 2);
		System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());

		if (node.Operand1.IsConstant)
		{
			opcodeEncoder.Append4Bits(0b0001);
			opcodeEncoder.Append2Bits(0b01);
			opcodeEncoder.EmitRelative26x4(node.BranchTarget1.Label);

			System.Diagnostics.Debug.Assert(opcodeEncoder.CheckOpcodeAlignment());
			return;
		}

		throw new Common.Exceptions.CompilerException($"Invalid Opcode: {node}");
	}
}
