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

using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Represents an operand, that is located on the relative to the current stack frame.
    /// </summary>
    public abstract class StackOperand : MemoryOperand, ICloneable
    {
        #region Data members

        /// <summary>
        /// Holds the SSA version of the stack operand.
        /// </summary>
        private int _ssaVersion;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="StackOperand"/>.
        /// </summary>
        /// <param name="type">Holds the type of data held in this operand.</param>
        /// <param name="register">Holds the stack frame register.</param>
        /// <param name="offset">The offset of the variable on stack. A positive value reflects the current function stack, a negative offset indicates a parameter.</param>
        protected StackOperand(SigType type, Register register, int offset) :
            base(type, register, new IntPtr(offset * 4))
        {
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the SSA version of the operand.
        /// </summary>
        public int Version
        {
            get { return _ssaVersion; }
            set { _ssaVersion = value; }
        }

        #endregion // Properties

        #region Operand Overrides

        public override string ToString()
        {
            string tmp = base.ToString();
            return tmp.Insert(tmp.Length-1, String.Format(", SSA Version: {0}", _ssaVersion));
        }

        #endregion // Operand Overrides

        #region ICloneable Members

        /// <summary>
        /// Clones the stack operand.
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        #endregion // ICloneable Members
    }
}
