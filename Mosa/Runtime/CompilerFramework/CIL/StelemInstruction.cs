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
	/// Intermediate representation of the CIL stelem opcode family.
	/// </summary>
	public class StelemInstruction : NaryInstruction
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private SigType _typeRef;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public StelemInstruction(OpCode opcode)
			: base(opcode, 3)
		{
			switch (opcode) {
				case OpCode.Stelem_i1:
					_typeRef = new SigType(CilElementType.I1);
					break;
				case OpCode.Stelem_i2:
					_typeRef = new SigType(CilElementType.I2);
					break;
				case OpCode.Stelem_i4:
					_typeRef = new SigType(CilElementType.I4);
					break;
				case OpCode.Stelem_i8:
					_typeRef = new SigType(CilElementType.I8);
					break;
				case OpCode.Stelem_i:
					_typeRef = new SigType(CilElementType.I);
					break;
				case OpCode.Stelem_r4:
					_typeRef = new SigType(CilElementType.R4);
					break;
				case OpCode.Stelem_r8:
					_typeRef = new SigType(CilElementType.R8);
					break;
				case OpCode.Stelem_ref: // FIXME: Really object?
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
				throw new NotImplementedException();
				//_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Stelem(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{4} ; {0}[{1}] = ({3}){2}", instruction.Operand1, instruction.Operand2, instruction.Operand3, _typeRef, base.ToString());
		}

		#endregion Methods


	}
}
