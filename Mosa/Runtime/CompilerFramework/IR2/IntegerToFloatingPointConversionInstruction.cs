using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of an integral to floating point conversion operation.
    /// </summary>
    public sealed class IntegerToFloatingPointConversionInstruction : TwoOperandInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerToFloatingPointConversionInstruction"/> class.
		/// </summary>
        public IntegerToFloatingPointConversionInstruction()
        {          
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(ref InstructionData instruction)
        {
            return String.Format(@"IR.itofp {0}, {1} ; {0} = (fp){1}", instruction.Operand1, instruction.Operand2);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
