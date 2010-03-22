/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MkrefanyInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="MkrefanyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public MkrefanyInstruction(OpCode opcode)
			: base(opcode)
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

			// Retrieve a type reference from the immediate argument
			// FIXME: Limit the token types
			TokenTypes token;
			decoder.Decode(out token);
			throw new NotImplementedException();
			/*
				_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
				_results[0] = CreateResultOperand(MetadataTypeReference.FromName(decoder.Metadata, @"System", @"TypedReference"));
			 */
			// FIXME: Validate the operands
			// FIXME: Do verification
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Mkrefany(context);
		}

		#endregion Methods

	}
}
