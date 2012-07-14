/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;

using Mosa.Compiler.TypeSystem.Generic;

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

			var token = decoder.DecodeTokenType();
			ctx.RuntimeField = decoder.Method.Module.GetField(token);

			if (ctx.RuntimeField.ContainsGenericParameter || ctx.RuntimeField.DeclaringType.ContainsOpenGenericParameters)
			{
				ctx.RuntimeField = decoder.GenericTypePatcher.PatchField(decoder.TypeModule, decoder.Method.DeclaringType as CilGenericType, ctx.RuntimeField);
				decoder.Compiler.Scheduler.TrackFieldReferenced(ctx.RuntimeField);
				Debug.Assert(!ctx.RuntimeField.ContainsGenericParameter);
			}

			var sigType = ctx.RuntimeField.SignatureType;
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, Operand.StackTypeFromSigType(sigType), sigType);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldfld(context);
		}

		#endregion Methods

	}
}
