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
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of a branch instruction.
    /// </summary>
    public sealed class JnsBranchInstruction : IR.OneOperandInstruction, Mosa.Runtime.CompilerFramework.IBranchInstruction
    {
        #region Data members
        /// <summary>
        /// Holds the branch target label.
        /// </summary>
        private int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="JnsBranchInstruction"/> class.
        /// </summary>
        public JnsBranchInstruction()
        {
            _label = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JnsBranchInstruction"/> class.
        /// </summary>
        /// <param name="label">The destination label.</param>
        public JnsBranchInstruction(int label)
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

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
            return String.Format(@"IR br.>= 0 {0} ; if >= 0 goto {0}", _label);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86 = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86)
                x86.Jns(this, arg);
        }

        #endregion // OneOperandInstruction Overrides

        #region IBranchInstruction Members

        int Mosa.Runtime.CompilerFramework.IBranchInstruction.Offset
        {
            get { return base.Offset; }
        }

        bool Mosa.Runtime.CompilerFramework.IBranchInstruction.IsConditional
        {
            get { return true; }
        }

        int[] Mosa.Runtime.CompilerFramework.IBranchInstruction.BranchTargets
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
