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

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of a branch context.
    /// </summary>
    public sealed class BranchInstruction : OneOperandInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the condition code to check in the branch.
        /// </summary>
        private ConditionCode _conditionCode;

        /// <summary>
        /// Holds the branch target label.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchInstruction"/> class.
        /// </summary>
        public BranchInstruction()
        {
            _label = 0;
            _conditionCode = ConditionCode.Equal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchInstruction"/> class.
        /// </summary>
        /// <param name="cc">The condition code to branch upon.</param>
        /// <param name="label">The destination label.</param>
        public BranchInstruction(ConditionCode cc, int label)
        {
            _conditionCode = cc;
            _label = label;
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

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>The label.</value>
        public int Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #endregion // Properties

        #region OneOperandInstruction Overrides

		/// <summary>
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
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
            return String.Format(@"IR.br.{0} {1} ; if {0} goto {1}", cc, _label);
        }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
            visitor.BranchInstruction(context);
        }

        #endregion // OneOperandInstruction Overrides

    }
}
