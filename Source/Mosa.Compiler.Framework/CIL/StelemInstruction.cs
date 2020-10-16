// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation of the CIL stelem opcode family.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.NaryInstruction" />
	public sealed class StelemInstruction : NaryInstruction
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
			switch (opcode)
			{
				case OpCode.Stelem_i1: elementType = MosaTypeCode.I1; break;
				case OpCode.Stelem_i2: elementType = MosaTypeCode.I2; break;
				case OpCode.Stelem_i4: elementType = MosaTypeCode.I4; break;
				case OpCode.Stelem_i8: elementType = MosaTypeCode.I8; break;
				case OpCode.Stelem_i: elementType = MosaTypeCode.I; break;
				case OpCode.Stelem_r4: elementType = MosaTypeCode.R4; break;
				case OpCode.Stelem_r8: elementType = MosaTypeCode.R8; break;
				case OpCode.Stelem_ref: elementType = MosaTypeCode.Object; break;
				case OpCode.Stelem: elementType = null; break;
				default: throw new NotImplementCompilerException("Not implemented: " + opcode);
			}
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

			node.MosaType = (elementType == null)
				? (MosaType)decoder.Instruction.Operand
				: decoder.MethodCompiler.Compiler.GetTypeFromTypeCode(elementType.Value);
		}

		#endregion Methods
	}
}
