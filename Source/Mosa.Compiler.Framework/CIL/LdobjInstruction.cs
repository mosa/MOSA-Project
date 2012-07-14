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

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdobjInstruction : UnaryInstruction
	{
		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private readonly SigType typeRef;

		/// <summary>
		/// Initializes a new instance of the <see cref="LdobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdobjInstruction(OpCode opcode)
			: base(opcode, 1)
		{
			switch (opcode)
			{
				case OpCode.Ldind_i1:
					typeRef = BuiltInSigType.SByte;
					break;
				case OpCode.Ldind_i2:
					typeRef = BuiltInSigType.Int16;
					break;
				case OpCode.Ldind_i4:
					typeRef = BuiltInSigType.Int32;
					break;
				case OpCode.Ldind_i8:
					typeRef = BuiltInSigType.Int64;
					break;
				case OpCode.Ldind_u1:
					typeRef = BuiltInSigType.Byte;
					break;
				case OpCode.Ldind_u2:
					typeRef = BuiltInSigType.UInt16;
					break;
				case OpCode.Ldind_u4:
					typeRef = BuiltInSigType.UInt32;
					break;
				case OpCode.Ldind_i:
					typeRef = BuiltInSigType.IntPtr;
					break;
				case OpCode.Ldind_r4:
					typeRef = BuiltInSigType.Single;
					break;
				case OpCode.Ldind_r8:
					typeRef = BuiltInSigType.Double;
					break;
				case OpCode.Ldind_ref: // FIXME: Really object?
					typeRef = BuiltInSigType.Object;
					break;
				case OpCode.Ldobj: // FIXME
					typeRef = null; // BuiltInSigType.Object;
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

			SigType sigType = typeRef;

			// Do we have a type?
			if (sigType == null)
			{
				// No, retrieve a type reference from the immediate argument
				Token token = decoder.DecodeTokenType();
				sigType = new ClassSigType(token);
			}
			
			// Push the loaded value
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, Operand.StackTypeFromSigType(sigType), sigType);
			ctx.SigType = sigType;
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			// If we're ldind.i8, fix an IL deficiency that the result may be U8
			if (opcode == OpCode.Ldind_i8 && typeRef.Type == CilElementType.I8)
			{
				SigType opType = ctx.Operand1.Type;
				RefSigType rst = opType as RefSigType;
				PtrSigType ptr = opType as PtrSigType;

				if (rst != null && rst.ElementType.Type == CilElementType.U8
					|| ptr != null && ptr.ElementType.Type == CilElementType.U8)
				{
					ctx.Result = compiler.CreateVirtualRegister(BuiltInSigType.UInt64);
				}
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldobj(context);
		}
	}
}
