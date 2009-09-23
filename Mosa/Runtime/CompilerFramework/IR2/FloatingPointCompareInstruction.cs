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

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Represents a floating point comparison instruction.
    /// </summary>
    public sealed class FloatingPointCompareInstruction : ThreeOperandInstruction
    {      

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatingPointCompareInstruction"/> class.
        /// </summary>
        public FloatingPointCompareInstruction()
        {
        }

        #endregion // Construction

        #region ThreeOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(ref InstructionData instruction)
        {
			// FIXME PG - pull from instructiondata ?
			ConditionCode _conditionCode = ConditionCode.Equal; // DUMMY

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
            return String.Format(@"IR fcmp {0} = {1} {2} {3}", instruction.Operand1, instruction.Operand2, cc, instruction.Operand3);
        }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.FloatingPointCompareInstruction(context);
        }

        #endregion // ThreeOperandInstruction Overrides
    }
}
