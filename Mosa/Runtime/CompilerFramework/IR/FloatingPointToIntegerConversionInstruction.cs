using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a floating point to integral conversion operation.
    /// </summary>
    public sealed class FloatingPointToIntegerConversionInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointToIntegerConversionInstruction"/> class.
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="src">The source operand.</param>
        public FloatingPointToIntegerConversionInstruction(Operand dest, Operand src) :
            base(dest, src)
        {
            Debug.Assert(dest.StackType == StackTypeCode.Int32 || dest.StackType == StackTypeCode.Int64 || dest.StackType == StackTypeCode.Ptr || dest.StackType == StackTypeCode.N, @"Invalid destination type.");
            if (dest.StackType != StackTypeCode.Int32 && dest.StackType != StackTypeCode.Int64 && dest.StackType == StackTypeCode.N && dest.StackType != StackTypeCode.Ptr)
                throw new ArgumentException(@"Invalid destination type.", @"dest");
            if (src.StackType != StackTypeCode.F)
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
            return String.Format(@"IR fptoi {0}, {1} ; {0} = (integer){1}", this.Operand0, this.Operand1);
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
