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

namespace Mosa.Runtime.CompilerFramework.CIL
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
		private SigType _valueType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StobjInstruction(OpCode opcode)
			: base(opcode)
		{
			switch (opcode) {
				case OpCode.Stind_i1:
					_valueType = new SigType(CilElementType.I1);
					break;
				case OpCode.Stind_i2:
					_valueType = new SigType(CilElementType.I2);
					break;
				case OpCode.Stind_i4:
					_valueType = new SigType(CilElementType.I4);
					break;
				case OpCode.Stind_i8:
					_valueType = new SigType(CilElementType.I8);
					break;
				case OpCode.Stind_r4:
					_valueType = new SigType(CilElementType.R4);
					break;
				case OpCode.Stind_r8:
					_valueType = new SigType(CilElementType.R8);
					break;
				case OpCode.Stind_i:
					_valueType = new SigType(CilElementType.I);
					break;
				case OpCode.Stind_ref: // FIXME: Really object?
					_valueType = new SigType(CilElementType.Object);
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
			if (_valueType == null) {
				// No, retrieve a type reference From the immediate argument
				TokenTypes token;
				decoder.Decode(out token);
				throw new NotImplementedException();
				//_valueType = MetadataTypeReference.FromToken(decoder.Metadata, token);
			}

			// FIXME: Check the value/destinations
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, IMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			SigType destType = ctx.Operand1.Type;

			Debug.Assert(destType is PtrSigType || destType is RefSigType, @"Destination operand not a pointer or reference.");
			if (!(destType is PtrSigType || destType is RefSigType))
				throw new ExecutionEngineException(@"Invalid operand.");
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
