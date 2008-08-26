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

		public ConversionInstruction(OpCode code)
			: base(code)
		{
		}

		#endregion // Construction

		#region Methods

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture arch = methodCompiler.Architecture;
            switch (this.Code)
            {
                case OpCode.Conv_i1:
                    return arch.CreateInstruction(typeof(IR.SConversionInstruction), this.Results[0], this.Operands[0]);

                case OpCode.Conv_i2:
                    return arch.CreateInstruction(typeof(IR.SConversionInstruction), this.Results[0], this.Operands[0]);

                case OpCode.Conv_i4:
                    return arch.CreateInstruction(typeof(IR.SConversionInstruction), this.Results[0], this.Operands[0]);

                case OpCode.Conv_i8:
                    return arch.CreateInstruction(typeof(IR.SConversionInstruction), this.Results[0], this.Operands[0]);

                default:
                    throw new NotImplementedException();
            }
        }

        public sealed override void Validate(MethodCompilerBase compiler)
        {
			// Validate the typecode & determine the resulting stack type
            SetResult(0, CreateResultOperand(compiler.Architecture, GetResultStackType()));
		}

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Conversion(this);
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
