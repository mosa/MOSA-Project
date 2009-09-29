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

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a branch instruction.
    /// </summary>
    public sealed class BranchInstruction : OneOperandInstruction, IBranchInstruction
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
            return String.Format(@"IR br.{0} {1} ; if {0} goto {1}", cc, _label);
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

        #endregion // OneOperandInstruction Overrides

        #region IBranchInstruction Members

        int IBranchInstruction.Offset
        {
            get { return base.Offset; }
        }

        bool IBranchInstruction.IsConditional
        {
            get { return true; }
        }

        int[] IBranchInstruction.BranchTargets
        {
            get { return new int[] { _label }; }
            set 
            {
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (value.Length == 0 || value.Length > 1)
                    throw new ArgumentException(@"Invalid array length.", @"value");

                _label = value[0];
            }
        }

        #endregion // IBranchInstruction Members
    }
}
