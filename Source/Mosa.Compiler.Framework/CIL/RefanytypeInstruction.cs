// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// FIXME: Validate operands & verify instruction
			ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.I4);
		}

		#endregion Methods
	}
}
