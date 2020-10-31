// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
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
			switch (opcode)
			{
				case OpCode.Ldelem_i1: elementType = MosaTypeCode.I1; break;
				case OpCode.Ldelem_i2: elementType = MosaTypeCode.I2; break;
				case OpCode.Ldelem_i4: elementType = MosaTypeCode.I4; break;
				case OpCode.Ldelem_i8: elementType = MosaTypeCode.I8; break;
				case OpCode.Ldelem_u1: elementType = MosaTypeCode.U1; break;
				case OpCode.Ldelem_u2: elementType = MosaTypeCode.U2; break;
				case OpCode.Ldelem_u4: elementType = MosaTypeCode.U4; break;
				case OpCode.Ldelem_i: elementType = MosaTypeCode.I; break;
				case OpCode.Ldelem_r4: elementType = MosaTypeCode.R4; break;
				case OpCode.Ldelem_r8: elementType = MosaTypeCode.R8; break;
				case OpCode.Ldelem_ref: elementType = MosaTypeCode.Object; break;
				default: elementType = null; break;
			}
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
}
