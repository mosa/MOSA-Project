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
	/// Intermediate representation of the CIL stsfld operation.
	/// </summary>
	public sealed class StsfldInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StsfldInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StsfldInstruction(OpCode opcode)
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
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var field = decoder.TypeSystem.Resolver.GetFieldByToken(decoder.Method.CodeAssembly, decoder.DecodeTokenType(), decoder.Method.DeclaringType.GenericArguments);

			decoder.Compiler.Scheduler.TrackFieldReferenced(field);

			ctx.MosaField = field;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Stsfld(context);
		}

		#endregion Methods
	}
}