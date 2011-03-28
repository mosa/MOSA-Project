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
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework.CIL
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

			//Console.WriteLine("Stfld used in {0}.{1}", decoder.Method.DeclaringType.FullName, decoder.Method.Name);

			Debug.Assert(token.Table == TableType.Field || token.Table == TableType.MemberRef, @"Invalid token type.");
			
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
