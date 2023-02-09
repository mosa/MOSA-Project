// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.CIL;

/// <summary>
/// Binary Branch Instruction
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.CIL.BinaryInstruction" />
public sealed class BinaryBranchInstruction : BinaryInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="BinaryBranchInstruction"/> class.
	/// </summary>
	/// <param name="opCode">The opcode.</param>
	public BinaryBranchInstruction(OpCode opCode)
		: base(opCode)
	{
	}

	#endregion Construction

	#region Properties

	/// <summary>
	/// Determines flow behavior of this instruction.
	/// </summary>
	/// <value></value>
	/// <remarks>
	/// Knowledge of control flow is required for correct basic block
	/// building. Any instruction that alters the control flow must override
	/// this property and correctly identify its control flow modifications.
	/// </remarks>
	public override FlowControl FlowControl { get { return FlowControl.ConditionalBranch; } }

	#endregion Properties

	#region Methods

	public override bool DecodeTargets(IInstructionDecoder decoder)
	{
		decoder.GetBlock((int)decoder.Instruction.Operand);
		return true;
	}

	/// <summary>
	/// Decodes the specified instruction.
	/// </summary>
	/// <param name="node">The context.</param>
	/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
	public override void Decode(InstructionNode node, IInstructionDecoder decoder)
	{
		// Decode base classes first
		base.Decode(node, decoder);

		var block = decoder.GetBlock((int)decoder.Instruction.Operand);

		node.AddBranchTarget(block);
	}

	public override string Modifier
	{
		get
		{
			return OpCode switch
			{
				OpCode.Beq_s => "==",
				OpCode.Beq => "==",
				OpCode.Bge_s => ">=",
				OpCode.Bge => ">=",
				OpCode.Bge_un_s => ">= unordered",
				OpCode.Bge_un => ">= unordered",
				OpCode.Bgt_s => ">",
				OpCode.Bgt => ">",
				OpCode.Bgt_un_s => "> unordered",
				OpCode.Bgt_un => "> unordered",
				OpCode.Ble_s => "<=",
				OpCode.Ble => "<=",
				OpCode.Ble_un_s => "<= unordered",
				OpCode.Ble_un => "<= unordered",
				OpCode.Blt_s => "<",
				OpCode.Blt => "<",
				OpCode.Blt_un_s => "< unordered",
				OpCode.Blt_un => "< unordered",
				OpCode.Bne_un_s => "!= unordered",
				OpCode.Bne_un => "!= unordered",
				_ => throw new InvalidOperationException("Opcode not set.")
			};
		}
	}

	#endregion Methods
}
