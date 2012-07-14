/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */



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

		#endregion // Construction

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
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode the base first
			base.Decode(ctx, decoder);

			ushort argIdx;

			// Opcode specific handling 
			if (opcode == OpCode.Starg_s)
			{
				byte arg = decoder.DecodeByte();
				argIdx = arg;
			}
			else
			{
				argIdx = decoder.DecodeUShort();
			}

			// The argument is the result
			ctx.Result = decoder.Compiler.GetParameterOperand(argIdx);

			// FIXME: Do some type compatibility checks
			// See verification for this instruction and
			// verification types.
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Starg(context);
		}

		#endregion Methods

	}
}
