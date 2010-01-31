using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR
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

        #endregion // TwoOperandInstruction Overrides
    }
}
