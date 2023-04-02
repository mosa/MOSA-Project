// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Intermediate representation of the CIL stelem opcode family.
/// </summary>
/// <seealso cref="NaryInstruction" />
internal sealed class StelemInstruction : NaryInstruction
{
	private readonly MosaTypeCode? elementType;

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="StelemInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public StelemInstruction(OpCode opcode)
		: base(opcode, 3)
	{
		elementType = opcode switch
		{
			OpCode.Stelem_i1 => MosaTypeCode.I1,
			OpCode.Stelem_i2 => MosaTypeCode.I2,
			OpCode.Stelem_i4 => MosaTypeCode.I4,
			OpCode.Stelem_i8 => MosaTypeCode.I8,
			OpCode.Stelem_i => MosaTypeCode.I,
			OpCode.Stelem_r4 => MosaTypeCode.R4,
			OpCode.Stelem_r8 => MosaTypeCode.R8,
			OpCode.Stelem_ref => MosaTypeCode.Object,
			OpCode.Stelem => null,
			_ => throw new NotImplementCompilerException("Not implemented: " + opcode)
		};
	}

	#endregion Construction

	#region Methods

	/// <summary>
	/// Decodes the specified instruction.
	/// </summary>
	/// <param name="node">The context.</param>
	/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
	public override void Decode(InstructionNode node, IInstructionDecoder decoder)
	{
		// Decode base classes first
		base.Decode(node, decoder);

		node.MosaType = elementType == null
			? (MosaType)decoder.Instruction.Operand
			: decoder.MethodCompiler.Compiler.GetTypeFromTypeCode(elementType.Value);
	}

	#endregion Methods
}
