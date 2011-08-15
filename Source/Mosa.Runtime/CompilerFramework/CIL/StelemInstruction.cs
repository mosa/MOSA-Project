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
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Intermediate representation of the CIL stelem opcode family.
	/// </summary>
	public sealed class StelemInstruction : NaryInstruction
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private readonly SigType typeRef;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StelemInstruction(OpCode opcode) : base (opcode, 3)
		{
			switch (opcode)
			{
				case OpCode.Stelem_i1:
					typeRef = new SigType(CilElementType.I1);
					break;
				case OpCode.Stelem_i2:
					typeRef = new SigType(CilElementType.I2);
					break;
				case OpCode.Stelem_i4:
					typeRef = new SigType(CilElementType.I4);
					break;
				case OpCode.Stelem_i8:
					typeRef = new SigType(CilElementType.I8);
					break;
				case OpCode.Stelem_i:
					typeRef = new SigType(CilElementType.I);
					break;
				case OpCode.Stelem_r4:
					typeRef = new SigType(CilElementType.R4);
					break;
				case OpCode.Stelem_r8:
					typeRef = new SigType(CilElementType.R8);
					break;
				case OpCode.Stelem_ref: // FIXME: Really object?
					typeRef = new SigType(CilElementType.Object);
					break;
				case OpCode.Stelem:
					typeRef = null;
					break;
				default:
					throw new NotImplementedException("Not implemented: " + opcode);
			}
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

			if (typeRef == null)
			{
				Token token = decoder.DecodeTokenType();
				RuntimeType type = decoder.TypeModule.GetType(token);

				ctx.Other = type;
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Stelem(context);
		}

		#endregion Methods

	}
}
