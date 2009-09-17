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
	/// Implements the internal representation for the IL conversion instructions.
	/// </summary>
	public class ConversionInstruction : UnaryArithmeticInstruction
	{
		#region Data members

		// FIXME
		private static StackTypeCode[] _conversionTable = new StackTypeCode[] {

		};

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConversionInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ConversionInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction



		#region CILInstruction Overrides

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(ref InstructionData instruction, IMethodCompiler compiler)
		{
			base.Validate(ref instruction, compiler);

			// Validate the typecode & determine the resulting stack type
			SigType resultType;

			switch (_opcode) {
				case OpCode.Conv_u: goto case OpCode.Conv_i;
				case OpCode.Conv_i:
					resultType = compiler.Architecture.NativeType;
					break;

				case OpCode.Conv_i1:
					resultType = new SigType(CilElementType.I1);
					break;

				case OpCode.Conv_i2:
					resultType = new SigType(CilElementType.I2);
					break;

				case OpCode.Conv_i4:
					resultType = new SigType(CilElementType.I4);
					break;

				case OpCode.Conv_i8:
					resultType = new SigType(CilElementType.I8);
					break;

				case OpCode.Conv_r4:
					resultType = new SigType(CilElementType.R4);
					break;

				case OpCode.Conv_r8:
					resultType = new SigType(CilElementType.R8);
					break;

				case OpCode.Conv_u1:
					resultType = new SigType(CilElementType.U1);
					break;

				case OpCode.Conv_u2:
					resultType = new SigType(CilElementType.U2);
					break;

				case OpCode.Conv_u4:
					resultType = new SigType(CilElementType.U4);
					break;

				case OpCode.Conv_u8:
					resultType = new SigType(CilElementType.U8);
					break;

				default:
					throw new NotSupportedException(@"Overflow checking conversions not supported.");
			}

			instruction.Result = compiler.CreateTemporary(resultType);
		}

		#endregion // CILInstruction Overrides

	}
}
