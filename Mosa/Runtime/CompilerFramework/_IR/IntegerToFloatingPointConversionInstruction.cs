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
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        public IntegerToFloatingPointConversionInstruction(Operand dest, Operand src) :
            base(dest, src)
        {
            Debug.Assert(dest.StackType == StackTypeCode.F, @"Invalid destination type.");
            if (dest.StackType != StackTypeCode.F)
                throw new ArgumentException(@"Invalid destination type.", @"dest");
            if (src.StackType != StackTypeCode.Int32 && src.StackType != StackTypeCode.Int64 && src.StackType != StackTypeCode.N && src.StackType != StackTypeCode.Ptr)
                throw new ArgumentException(@"Invalid source type.", @"src");
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"IR itofp {0}, {1} ; {0} = (fp){1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
