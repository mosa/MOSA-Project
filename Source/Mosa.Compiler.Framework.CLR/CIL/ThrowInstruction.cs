// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Intermediate representation of the IL throw instruction.
/// </summary>
/// <seealso cref="UnaryInstruction" />
internal sealed class ThrowInstruction : UnaryInstruction
{
	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="ThrowInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public ThrowInstruction(OpCode opcode)
		: base(opcode)
	{
	}

	#endregion Construction

	#region Properties

	public override void Decode(InstructionNode node, IInstructionDecoder decoder)
	{
		base.Decode(node, decoder);
	}

	/// <summary>
	/// Determines flow behavior of this instruction.
	/// </summary>
	/// <value></value>
	/// <remarks>
	/// Knowledge of control flow is required for correct basic block
	/// building. Any instruction that alters the control flow must override
	/// this property and correctly identify its control flow modifications.
	/// </remarks>
	public override FlowControl FlowControl => FlowControl.Throw;

	#endregion Properties
}
