/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.CIL
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
					typeRef = BuiltInSigType.SByte;
					break;
				case OpCode.Stelem_i2:
					typeRef = BuiltInSigType.Int16;
					break;
				case OpCode.Stelem_i4:
					typeRef = BuiltInSigType.Int32;
					break;
				case OpCode.Stelem_i8:
					typeRef = BuiltInSigType.Int64;
					break;
				case OpCode.Stelem_i:
					typeRef = BuiltInSigType.IntPtr;
					break;
				case OpCode.Stelem_r4:
					typeRef = BuiltInSigType.Single;
					break;
				case OpCode.Stelem_r8:
					typeRef = BuiltInSigType.Double;
					break;
				case OpCode.Stelem_ref: // FIXME: Really object?
					typeRef = BuiltInSigType.Object;
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

				ctx.RuntimeType = type;
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
