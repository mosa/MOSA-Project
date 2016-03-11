// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation for the CIL stfld opcode.
	/// </summary>
	public sealed class StfldInstruction : BinaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StfldInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Methods Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var field = (MosaField)decoder.Instruction.Operand;

			decoder.Compiler.Scheduler.TrackFieldReferenced(field);

			ctx.MosaField = field;
		}

		#endregion Methods Overrides
	}
}
