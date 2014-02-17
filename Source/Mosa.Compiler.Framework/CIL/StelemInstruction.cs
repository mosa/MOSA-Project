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
	/// Intermediate representation of the CIL stelem opcode family.
	/// </summary>
	public sealed class StelemInstruction : NaryInstruction
	{
		private readonly ElementType? elementType;

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StelemInstruction(OpCode opcode)
			: base(opcode, 3)
		{
			switch (opcode)
			{
				case OpCode.Stelem_i1: elementType = ElementType.I1; break;
				case OpCode.Stelem_i2: elementType = ElementType.I2; break;
				case OpCode.Stelem_i4: elementType = ElementType.I4; break;
				case OpCode.Stelem_i8: elementType = ElementType.I8; break;
				case OpCode.Stelem_i: elementType = ElementType.I; break;
				case OpCode.Stelem_r4: elementType = ElementType.R4; break;
				case OpCode.Stelem_r8: elementType = ElementType.R8; break;
				case OpCode.Stelem_ref: elementType = ElementType.Object; break;
				case OpCode.Stelem: elementType = null; break;
				default: throw new NotImplementedException("Not implemented: " + opcode);
			}
		}

		#endregion Construction

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

			MosaType type = (elementType == null)
				? type = (MosaType)decoder.Instruction.Operand
				: type = decoder.TypeSystem.GetTypeFromElementCode(elementType.Value);

			ctx.MosaType = type;
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