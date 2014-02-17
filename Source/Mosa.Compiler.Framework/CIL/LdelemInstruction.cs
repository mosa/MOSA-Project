/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem;
using System;

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
		private readonly ElementType? elementType;

		/// <summary>
		/// Initializes a new instance of the <see cref="LdelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdelemInstruction(OpCode opcode)
			: base(opcode, 1)
		{
			switch (opcode)
			{
				case OpCode.Ldelem_i1: elementType = ElementType.I1; break;
				case OpCode.Ldelem_i2: elementType = ElementType.I2; break;
				case OpCode.Ldelem_i4: elementType = ElementType.I4; break;
				case OpCode.Ldelem_i8: elementType = ElementType.I8; break;
				case OpCode.Ldelem_u1: elementType = ElementType.U1; break;
				case OpCode.Ldelem_u2: elementType = ElementType.U2; break;
				case OpCode.Ldelem_u4: elementType = ElementType.U4; break;
				case OpCode.Ldelem_i: elementType = ElementType.I; break;
				case OpCode.Ldelem_r4: elementType = ElementType.R4; break;
				case OpCode.Ldelem_r8: elementType = ElementType.R8; break;
				case OpCode.Ldelem_ref: elementType = ElementType.Object; break;
				default: throw new NotImplementedException();
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

			MosaType type = (elementType == null)
				? type = (MosaType)decoder.Instruction.Operand
				: type = decoder.TypeSystem.GetTypeFromElementCode(elementType.Value);

			ctx.Result = LoadInstruction.CreateResultOperand(decoder, type);
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