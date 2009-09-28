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
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
			return String.Format(@"IR.itofp {0}, {1} ; {0} = (fp){1}", context.Operand1, context.Operand2);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
