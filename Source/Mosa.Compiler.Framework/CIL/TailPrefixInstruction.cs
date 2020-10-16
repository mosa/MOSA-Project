// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Tail Prefix Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.PrefixInstruction" />
	public sealed class TailPrefixInstruction : PrefixInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TailPrefixInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public TailPrefixInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Methods Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(node, decoder);
		}

		#endregion Methods Overrides
	}
}
