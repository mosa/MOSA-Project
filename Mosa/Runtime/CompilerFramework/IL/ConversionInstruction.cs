/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
	/// <summary>
	/// Implements the internal representation for the IL conversion instructions.
	/// </summary>
	public class ConversionInstruction : UnaryArithmeticInstruction {

		#region Data members

		// FIXME
		private static StackTypeCode[] _conversionTable = new StackTypeCode[] {

		};

		#endregion // Data members

		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
		public ConversionInstruction(OpCode code)
			: base(code)
		{
		}

		#endregion // Construction

		#region Methods

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public sealed override void Validate(MethodCompilerBase compiler)
        {
			// Validate the typecode & determine the resulting stack type
            SigType resultType;
            switch (this.Code)
            {
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

            SetResult(0, compiler.CreateResultOperand(resultType));
		}

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Conversion(this, arg);
        }

		private StackTypeCode GetResultStackType()
		{
			StackTypeCode result;
			switch (_code)
			{
				case OpCode.Conv_i: result = StackTypeCode.N; break;
				case OpCode.Conv_i1: result = StackTypeCode.Int32; break;
				case OpCode.Conv_i2: result = StackTypeCode.Int32; break;
				case OpCode.Conv_i4: result = StackTypeCode.Int32; break;
				case OpCode.Conv_i8: result = StackTypeCode.Int64; break;
				case OpCode.Conv_ovf_i: result = StackTypeCode.N; break;
				case OpCode.Conv_ovf_i_un: result = StackTypeCode.N; break;
				case OpCode.Conv_ovf_i1: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i1_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i2: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i2_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i4: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i4_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_i8: result = StackTypeCode.Int64; break;
				case OpCode.Conv_ovf_i8_un: result = StackTypeCode.Int64; break;
				case OpCode.Conv_ovf_u: result = StackTypeCode.N; break;
				case OpCode.Conv_ovf_u_un: result = StackTypeCode.N; break;
				case OpCode.Conv_ovf_u1: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u1_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u2: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u2_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u4: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u4_un: result = StackTypeCode.Int32; break;
				case OpCode.Conv_ovf_u8: result = StackTypeCode.Int64; break;
				case OpCode.Conv_ovf_u8_un: result = StackTypeCode.Int64; break;
				case OpCode.Conv_r_un: result = StackTypeCode.F; break;
				case OpCode.Conv_r4: result = StackTypeCode.F; break;
				case OpCode.Conv_r8: result = StackTypeCode.F; break;
				case OpCode.Conv_u: result = StackTypeCode.N; break;
				case OpCode.Conv_u1: result = StackTypeCode.Int32; break;
				case OpCode.Conv_u2: result = StackTypeCode.Int32; break;
				case OpCode.Conv_u4: result = StackTypeCode.Int32; break;
				case OpCode.Conv_u8: result = StackTypeCode.Int64; break;
				default:
					throw new NotImplementedException(@"Unsupported conversion opcode.");
			}
			return result;
		}

		#endregion // Methods
	}
}
