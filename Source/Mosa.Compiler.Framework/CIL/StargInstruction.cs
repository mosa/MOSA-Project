// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class StargInstruction : StoreInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StargInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StargInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Starg has a result, but doesn't push it on the stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public override bool PushResult
		{
			get { return false; }
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode ctx, IInstructionDecoder decoder)
		{
			// Decode the base first
			base.Decode(ctx, decoder);

			// The argument is the result
			ctx.Result = decoder.Compiler.GetParameterOperand((int)decoder.Instruction.Operand);

			// FIXME: Do some type compatibility checks
			// See verification for this instruction and
			// verification types.
		}

		#endregion Methods
	}
}
