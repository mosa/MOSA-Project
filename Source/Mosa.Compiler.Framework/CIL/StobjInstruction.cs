/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation for stobj and stind.* IL instructions.
	/// </summary>
	public sealed class StobjInstruction : BinaryInstruction
	{
		#region Data members

		/// <summary>
		/// Specifies the type of the value.
		/// </summary>
		private readonly CilElementType? elementType;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StobjInstruction(OpCode opcode)
			: base(opcode)
		{
			switch (opcode)
			{
				case OpCode.Stind_i1: elementType = CilElementType.I1; break;
				case OpCode.Stind_i2: elementType = CilElementType.I2; break;
				case OpCode.Stind_i4: elementType = CilElementType.I4; break;
				case OpCode.Stind_i8: elementType = CilElementType.I8; break;
				case OpCode.Stind_r4: elementType = CilElementType.R4; break;
				case OpCode.Stind_r8: elementType = CilElementType.R8; break;
				case OpCode.Stind_i: elementType = CilElementType.I; break;
				case OpCode.Stind_ref: elementType = CilElementType.Object; break;
				case OpCode.Stobj: elementType = null; break;
				default: throw new NotImplementedException();
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
				? type = decoder.TypeSystem.Resolver.GetTypeByToken(decoder.Method.CodeAssembly, decoder.DecodeTokenType(), decoder.Method)
				: type = decoder.TypeSystem.Resolver.GetTypeByElementType(elementType);

			ctx.MosaType = type;

			// FIXME: Check the value/destinations
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context ctx, BaseMethodCompiler compiler)
		{
			base.Resolve(ctx, compiler);

			//FIXME: Intent?
			//SigType destType = ctx.Operand1.Type;

			//Debug.Assert(destType is PtrSigType || destType is RefSigType, @"Destination operand not a pointer or reference.");
			//if (!(destType is PtrSigType || destType is RefSigType))
			//    throw new InvalidOperationException(@"Invalid operand.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Stobj(context);
		}

		#endregion Methods
	}
}