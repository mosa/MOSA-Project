// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation of the localalloc IL instruction.
	/// </summary>
	public sealed class LocalallocInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalallocInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LocalallocInstruction(OpCode opcode)
			: base(opcode)
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

			// Push the address on the stack
			ctx.Result = decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.U);
		}

		#endregion Methods
	}
}
