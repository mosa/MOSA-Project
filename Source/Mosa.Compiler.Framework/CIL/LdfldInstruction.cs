// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdfldInstruction : BaseCILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdfldInstruction(OpCode opcode)
			: base(opcode, 1, 1)
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

			var field = (MosaField)decoder.Instruction.Operand;

			decoder.Compiler.Scheduler.TrackFieldReferenced(field);

			ctx.MosaField = field;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, field.FieldType);
		}

		#endregion Methods
	}
}
