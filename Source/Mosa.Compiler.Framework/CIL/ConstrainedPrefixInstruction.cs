// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class ConstrainedPrefixInstruction : PrefixInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstrainedPrefixInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ConstrainedPrefixInstruction(OpCode opcode)
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
			var type = (MosaType)decoder.Instruction.Operand;

			ctx.MosaType = type;
		}

		#endregion Methods
	}
}