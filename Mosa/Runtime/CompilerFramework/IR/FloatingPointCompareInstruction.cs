/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Represents a floating point comparison instruction.
    /// </summary>
    public sealed class FloatingPointCompareInstruction : ThreeOperandInstruction, IConditionalInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the conditional code of the comparison.
        /// </summary>
        private ConditionCode _conditionCode;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointCompareInstruction"/> class.
        /// </summary>
        public FloatingPointCompareInstruction()
        {
            _conditionCode = ConditionCode.Equal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointCompareInstruction"/> class.
        /// </summary>
        /// <param name="dest">The destination operand.</param>
        /// <param name="op1">The first operand to compare.</param>
        /// <param name="code">The condition code.</param>
        /// <param name="op2">The second operand to compare.</param>
        public FloatingPointCompareInstruction(Operand dest, Operand op1, ConditionCode code, Operand op2) :
            base(dest, op1, op2)
        {
            _conditionCode = code;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the condition code.
        /// </summary>
        /// <value>The condition code.</value>
        public ConditionCode ConditionCode
        {
            get { return _conditionCode; }
            set { _conditionCode = value; }
        }

        #endregion // Properties

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            string cc;
            switch (_conditionCode)
            {
                case ConditionCode.Equal: cc = @"=="; break;
                case ConditionCode.GreaterOrEqual: cc = @">="; break;
                case ConditionCode.GreaterThan: cc = @">"; break;
                case ConditionCode.LessOrEqual: cc = @"<="; break;
                case ConditionCode.LessThan: cc = @"<"; break;
                case ConditionCode.NotEqual: cc = @"!="; break;
                case ConditionCode.UnsignedGreaterOrEqual: cc = @">= (U)"; break;
                case ConditionCode.UnsignedGreaterThan: cc = @"> (U)"; break;
                case ConditionCode.UnsignedLessOrEqual: cc = @"<= (U)"; break;
                case ConditionCode.UnsignedLessThan: cc = @"< (U)"; break;
                default:
                    throw new NotSupportedException();
            } 
            return String.Format(@"IR fcmp {0} = {1} {2} {3}", this.Operand0, this.Operand1, cc, this.Operand2);
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

        #endregion // ThreeOperandInstruction Overrides
    }
}
