// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL;

/// <summary>
/// Ldelem Instruction
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.CIL.BinaryInstruction" />
public sealed class LdelemInstruction : BinaryInstruction
{
	/// <summary>
	/// A fixed typeref for ldind.* instructions.
	/// </summary>
	private readonly MosaTypeCode? elementType;

	/// <summary>
	/// Initializes a new instance of the <see cref="LdelemInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public LdelemInstruction(OpCode opcode)
		: base(opcode, 1)
	{
		elementType = opcode switch
		{
			OpCode.Ldelem_i1 => MosaTypeCode.I1,
			OpCode.Ldelem_i2 => MosaTypeCode.I2,
			OpCode.Ldelem_i4 => MosaTypeCode.I4,
			OpCode.Ldelem_i8 => MosaTypeCode.I8,
			OpCode.Ldelem_u1 => MosaTypeCode.U1,
			OpCode.Ldelem_u2 => MosaTypeCode.U2,
			OpCode.Ldelem_u4 => MosaTypeCode.U4,
			OpCode.Ldelem_i => MosaTypeCode.I,
			OpCode.Ldelem_r4 => MosaTypeCode.R4,
			OpCode.Ldelem_r8 => MosaTypeCode.R8,
			OpCode.Ldelem_ref => MosaTypeCode.Object,
			_ => null
		};
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

		var type = (elementType == null)
			? (MosaType)decoder.Instruction.Operand
			: decoder.MethodCompiler.Compiler.GetTypeFromTypeCode(elementType.Value);

		node.Result = decoder.MethodCompiler.AllocateVirtualRegisterOrStackSlot(type);
	}
}
