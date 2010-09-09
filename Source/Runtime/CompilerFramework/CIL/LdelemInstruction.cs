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

using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdelemInstruction : BinaryInstruction
	{
		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private SigType elementType;

		/// <summary>
		/// Initializes a new instance of the <see cref="LdelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdelemInstruction(OpCode opcode)
			: base(opcode)
		{
			switch (opcode)
			{
				case OpCode.Ldelem_i1:
					this.elementType = BuiltInSigType.SByte;
					break;
				case OpCode.Ldelem_i2:
					this.elementType = BuiltInSigType.Int16;
					break;
				case OpCode.Ldelem_i4:
					this.elementType = BuiltInSigType.Int32;
					break;
				case OpCode.Ldelem_i8:
					this.elementType = BuiltInSigType.Int64;
					break;
				case OpCode.Ldelem_u1:
					this.elementType = BuiltInSigType.Byte;
					break;
				case OpCode.Ldelem_u2:
					this.elementType = BuiltInSigType.UInt16;
					break;
				case OpCode.Ldelem_u4:
					this.elementType = BuiltInSigType.UInt32;
					break;
				case OpCode.Ldelem_i:
					this.elementType = BuiltInSigType.IntPtr;
					break;
				case OpCode.Ldelem_r4:
					this.elementType = BuiltInSigType.Single;
					break;
				case OpCode.Ldelem_r8:
					this.elementType = BuiltInSigType.Double;
					break;
				case OpCode.Ldelem_ref: // FIXME: Really object?
					this.elementType = BuiltInSigType.Object;
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
		/// <param name="typeSystem">The type system.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder, ITypeSystem typeSystem)
		{
			// Decode base classes first
			base.Decode(ctx, decoder, typeSystem);

			// Do we have a type?
			if (this.elementType == null)
			{
				// No, retrieve a type reference from the immediate argument
				TokenTypes token = decoder.DecodeTokenType();
				this.elementType = new ClassSigType(token);
			}

			StackTypeCode stackType = Operand.StackTypeFromSigType(this.elementType);
			Operand result = LoadInstruction.CreateResultOperand(decoder, stackType, this.elementType);

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
