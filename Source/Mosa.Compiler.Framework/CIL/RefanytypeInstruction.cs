// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Refanytype Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.UnaryInstruction" />
	public sealed class RefanytypeInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RefanytypeInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public RefanytypeInstruction(OpCode opcode)
			: base(opcode, 1)
		{
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

			// FIXME: Validate operands & verify instruction
			node.Result = decoder.MethodCompiler.CreateVirtualRegister(decoder.MethodCompiler.Architecture.Is32BitPlatform ? decoder.TypeSystem.BuiltIn.I4 : decoder.TypeSystem.BuiltIn.I8);
		}

		#endregion Methods
	}
}
