/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem.Generic;

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

		#endregion // Construction

		#region Methods Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Load the _stackFrameIndex token from the immediate
			Token token = decoder.DecodeTokenType();
			Debug.Assert(token.Table == TableType.Field || token.Table == TableType.MemberRef, @"Invalid token type.");

			ctx.RuntimeField = decoder.Method.Module.GetField(token);

			if (ctx.RuntimeField.ContainsGenericParameter || ctx.RuntimeField.DeclaringType.ContainsOpenGenericParameters)
			{
				ctx.RuntimeField = decoder.GenericTypePatcher.PatchField(decoder.TypeModule, decoder.Method.DeclaringType as CilGenericType, ctx.RuntimeField);
			}
			decoder.Compiler.Scheduler.TrackFieldReferenced(ctx.RuntimeField);

			Debug.Assert(!ctx.RuntimeField.ContainsGenericParameter);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Stfld(context);
		}

		#endregion // Methods Overrides
	}
}
