/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdfldInstruction : BaseInstruction
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

			Token token = decoder.DecodeTokenType();
			ITypeModule module = null;
			Mosa.Runtime.TypeSystem.Generic.CilGenericType genericType = decoder.Method.DeclaringType as Mosa.Runtime.TypeSystem.Generic.CilGenericType;
			if (genericType != null)
				module = (decoder.Method.DeclaringType as Mosa.Runtime.TypeSystem.Generic.CilGenericType).BaseGenericType.Module;
			else
				module = decoder.Method.Module;
			ctx.RuntimeField = module.GetField(token);

			if (ctx.RuntimeField.ContainsGenericParameter)
			{
				foreach (RuntimeField field in decoder.Method.DeclaringType.Fields)
					if (field.Name == ctx.RuntimeField.Name)
					{
						ctx.RuntimeField = field;
						break;
					}

				Debug.Assert(!ctx.RuntimeField.ContainsGenericParameter);
			}

			SigType sigType = ctx.RuntimeField.SignatureType;
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
