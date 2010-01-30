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
	public sealed class LdelemInstruction : BinaryInstruction
	{
		#region Data members

		/// <summary>
		/// A fixed typeref for ldind.* instructions.
		/// </summary>
		private SigType _typeRef;

		#endregion // Data members
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdelemInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdelemInstruction(OpCode opcode)
			: base(opcode)
		{
			switch (opcode) {
				case OpCode.Ldelem_i1:
					_typeRef = new SigType(CilElementType.I1);
					break;
				case OpCode.Ldelem_i2:
					_typeRef = new SigType(CilElementType.I2);
					break;
				case OpCode.Ldelem_i4:
					_typeRef = new SigType(CilElementType.I4);
					break;
				case OpCode.Ldelem_i8:
					_typeRef = new SigType(CilElementType.I8);
					break;
				case OpCode.Ldelem_u1:
					_typeRef = new SigType(CilElementType.U1);
					break;
				case OpCode.Ldelem_u2:
					_typeRef = new SigType(CilElementType.U2);
					break;
				case OpCode.Ldelem_u4:
					_typeRef = new SigType(CilElementType.U4);
					break;
				case OpCode.Ldelem_i:
					_typeRef = new SigType(CilElementType.I);
					break;
				case OpCode.Ldelem_r4:
					_typeRef = new SigType(CilElementType.R4);
					break;
				case OpCode.Ldelem_r8:
					_typeRef = new SigType(CilElementType.R8);
					break;
				case OpCode.Ldelem_ref: // FIXME: Really object?
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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

            // Do we have a type?
            if (_typeRef == null)
            {
                // No, retrieve a type reference From the immediate argument
                TokenTypes token;
                decoder.Decode(out token);
                throw new NotImplementedException();
                //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
            }

            // Push the loaded value
			ctx.Result = decoder.Compiler.CreateTemporary(_typeRef);
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

		#endregion Methods
	}
}
