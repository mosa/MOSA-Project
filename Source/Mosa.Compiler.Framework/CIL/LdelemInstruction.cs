/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdelemInstruction : BinaryInstruction
	{
		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private readonly SigType elementType;

		/// <summary>
		/// Initializes a new instance of the <see cref="LdelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdelemInstruction(OpCode opcode)
			: base(opcode, 1)
		{
			switch (opcode)
			{
				case OpCode.Ldelem_i1:
					elementType = BuiltInSigType.SByte;
					break;
				case OpCode.Ldelem_i2:
					elementType = BuiltInSigType.Int16;
					break;
				case OpCode.Ldelem_i4:
					elementType = BuiltInSigType.Int32;
					break;
				case OpCode.Ldelem_i8:
					elementType = BuiltInSigType.Int64;
					break;
				case OpCode.Ldelem_u1:
					elementType = BuiltInSigType.Byte;
					break;
				case OpCode.Ldelem_u2:
					elementType = BuiltInSigType.UInt16;
					break;
				case OpCode.Ldelem_u4:
					elementType = BuiltInSigType.UInt32;
					break;
				case OpCode.Ldelem_i:
					elementType = BuiltInSigType.IntPtr;
					break;
				case OpCode.Ldelem_r4:
					elementType = BuiltInSigType.Single;
					break;
				case OpCode.Ldelem_r8:
					elementType = BuiltInSigType.Double;
					break;
				case OpCode.Ldelem_ref: // FIXME: Really object?
					elementType = BuiltInSigType.Object;
					break;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			SigType sigType = elementType;

			// Do we have a type?
			if (sigType == null)
			{
				// No, retrieve a type reference from the immediate argument
				Token token = decoder.DecodeTokenType();
				sigType = new ClassSigType(token);
			}

			StackTypeCode stackType = Operand.StackTypeFromSigType(sigType);
			Operand result = LoadInstruction.CreateResultOperand(decoder, stackType, sigType);

			ctx.Result = result;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldelem(context);
		}
	}
}
