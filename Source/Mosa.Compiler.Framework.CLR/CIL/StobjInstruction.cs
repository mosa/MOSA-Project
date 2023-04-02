// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CLR.CIL;

/// <summary>
/// Intermediate representation for stobj and stind.* IL instructions.
/// </summary>
/// <seealso cref="BinaryInstruction" />
internal sealed class StobjInstruction : BinaryInstruction
{
	#region Data Members

	/// <summary>
	/// Specifies the type of the value.
	/// </summary>
	private readonly MosaTypeCode? elementType;

	#endregion Data Members

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="StobjInstruction"/> class.
	/// </summary>
	/// <param name="opcode">The opcode.</param>
	public StobjInstruction(OpCode opcode)
		: base(opcode)
	{
		elementType = opcode switch
		{
			OpCode.Stind_i1 => MosaTypeCode.I1,
			OpCode.Stind_i2 => MosaTypeCode.I2,
			OpCode.Stind_i4 => MosaTypeCode.I4,
			OpCode.Stind_i8 => MosaTypeCode.I8,
			OpCode.Stind_r4 => MosaTypeCode.R4,
			OpCode.Stind_r8 => MosaTypeCode.R8,
			OpCode.Stind_i => MosaTypeCode.I,
			OpCode.Stind_ref => MosaTypeCode.Object,
			OpCode.Stobj => null,
			_ => throw new NotImplementCompilerException()
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

		// FIXME: Check the value/destinations
	}

	/// <summary>
	/// Validates the instruction operands and creates a matching variable for the result.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="methodCompiler">The compiler.</param>
	public override void Resolve(Context context, MethodCompiler methodCompiler)
	{
		base.Resolve(context, methodCompiler);
	}

	#endregion Methods
}
