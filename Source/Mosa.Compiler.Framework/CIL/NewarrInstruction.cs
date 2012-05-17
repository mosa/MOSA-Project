/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

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

		#endregion // Construction

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

			// Read the type specification
			Token token = decoder.DecodeTokenType();

			// Patch usage of generic arguments
			var enclosingType = decoder.Method.DeclaringType;
			var signatureType = decoder.GenericTypePatcher.PatchSignatureType(decoder.TypeModule, enclosingType, token);

			// FIXME: If ctx.Operands1 is an integral constant, we can infer the maximum size of the array
			// and instantiate an ArrayTypeSpecification with max. sizes. This way we could eliminate bounds
			// checks in an optimization stage later on, if we find that a value never exceeds the array 
			// bounds.
			var resultType = new SZArraySigType(null, signatureType);
			ctx.Result = decoder.Compiler.CreateVirtualRegister(resultType);
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
