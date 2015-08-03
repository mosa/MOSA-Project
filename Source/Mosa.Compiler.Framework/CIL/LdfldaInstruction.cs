// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class LdfldaInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdfldaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdfldaInstruction(OpCode opcode)
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
			base.Decode(ctx, decoder);

			var field = (MosaField)decoder.Instruction.Operand;

			decoder.Compiler.Scheduler.TrackFieldReferenced(field);

			ctx.MosaField = field;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, field.FieldType.ToManagedPointer());
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldflda(context);
		}

		#endregion Methods
	}
}