// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class UnboxInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnboxInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnboxInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var type = (MosaType)decoder.Instruction.Operand;

			//Operand result = decoder.Compiler.CreateVirtualRegister(type);
			//ctx.Result = result;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, type);
			ctx.MosaType = type;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Unbox(context);
		}

		#endregion Methods
	}
}
