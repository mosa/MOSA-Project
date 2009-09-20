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
	/// 
	/// </summary>
	public class LdobjInstruction : UnaryInstruction
	{
		#region Data members

		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private SigType _typeRef;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdobjInstruction(OpCode opcode)
			: base(opcode, 1)
		{
			switch (opcode) {
				case OpCode.Ldind_i1:
					_typeRef = new SigType(CilElementType.I1);
					break;
				case OpCode.Ldind_i2:
					_typeRef = new SigType(CilElementType.I2);
					break;
				case OpCode.Ldind_i4:
					_typeRef = new SigType(CilElementType.I4);
					break;
				case OpCode.Ldind_i8:
					_typeRef = new SigType(CilElementType.I8);
					break;
				case OpCode.Ldind_u1:
					_typeRef = new SigType(CilElementType.U1);
					break;
				case OpCode.Ldind_u2:
					_typeRef = new SigType(CilElementType.U2);
					break;
				case OpCode.Ldind_u4:
					_typeRef = new SigType(CilElementType.U4);
					break;
				case OpCode.Ldind_i:
					_typeRef = new SigType(CilElementType.I);
					break;
				case OpCode.Ldind_r4:
					_typeRef = new SigType(CilElementType.R4);
					break;
				case OpCode.Ldind_r8:
					_typeRef = new SigType(CilElementType.R8);
					break;
				case OpCode.Ldind_ref: // FIXME: Really object?
					_typeRef = new SigType(CilElementType.Object);
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
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			// Do we have a type?
			if (_typeRef == null) {
				// No, retrieve a type reference From the immediate argument
				TokenTypes token;
				decoder.Decode(out token);
				//_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
			}

			// Push the loaded value
			instruction.Result = decoder.Compiler.CreateTemporary(_typeRef);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			// If we're ldind.i8, fix an IL deficiency that the result may be U8
			if (_opcode == OpCode.Ldind_i8 && _typeRef.Type == CilElementType.I8) {
				SigType opType = instruction.Operand1.Type;
				RefSigType rst = opType as RefSigType;
				PtrSigType ptr = opType as PtrSigType;

				if (rst != null && rst.ElementType.Type == CilElementType.U8 ||
					ptr != null && ptr.ElementType.Type == CilElementType.U8) {
					instruction.Result = compiler.CreateTemporary(new SigType(CilElementType.U8));
				}
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Ldobj(context);
		}

		#endregion Methods

	}
}
