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
	public sealed class NewarrInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NewarrInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NewarrInstruction(OpCode opcode)
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
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			var type = decoder.TypeSystem.Resolver.GetTypeByToken(decoder.Method.CodeAssembly, decoder.DecodeTokenType(), decoder.Method);

			// FIXME: If ctx.Operands1 is an integral constant, we can infer the maximum size of the array
			// and instantiate an ArrayTypeSpecification with max. sizes. This way we could eliminate bounds
			// checks in an optimization stage later on, if we find that a value never exceeds the array
			// bounds.

			var arrayType = decoder.TypeSystem.Resolver.GetArrayType(type);

			ctx.Result = decoder.Compiler.CreateVirtualRegister(arrayType);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Newarr(context);
		}

		#endregion Methods
	}
}