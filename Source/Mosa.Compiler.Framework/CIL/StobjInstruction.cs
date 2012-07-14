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
	/// Intermediate representation for stobj and stind.* IL instructions.
	/// </summary>
	public sealed class StobjInstruction : BinaryInstruction
	{
		#region Data members

		/// <summary>
		/// Specifies the type of the value.
		/// </summary>
		private readonly SigType valueType;

		#endregion // Data members

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
				case OpCode.Stind_i1:
					valueType = BuiltInSigType.SByte;
					break;
				case OpCode.Stind_i2:
					valueType = BuiltInSigType.Int16;
					break;
				case OpCode.Stind_i4:
					valueType = BuiltInSigType.Int32;
					break;
				case OpCode.Stind_i8:
					valueType = BuiltInSigType.Int64;
					break;
				case OpCode.Stind_r4:
					valueType = BuiltInSigType.Single;
					break;
				case OpCode.Stind_r8:
					valueType = BuiltInSigType.Double;
					break;
				case OpCode.Stind_i:
					valueType = BuiltInSigType.IntPtr;
					break;
				case OpCode.Stind_ref: // FIXME: Really object?
					valueType = BuiltInSigType.Object;
					break;
				case OpCode.Stobj:  // FIXME
					valueType = null;
					break;
				default:
					throw new NotImplementedException();
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

			// Do we have a type?
			if (valueType == null)
			{
				// No, retrieve a type reference from the immediate argument
				Token token = decoder.DecodeTokenType();
				RuntimeType type = decoder.TypeModule.GetType(token);

				ctx.RuntimeType = type;
			}

			// FIXME: Check the value/destinations
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, BaseMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

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

		#endregion // Methods
	}
}
