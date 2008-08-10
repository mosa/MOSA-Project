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
    /// Specifies a memory operand defined by an offset and an optional base register.
    /// </summary>
    public class MemoryOperand : Operand
    {
        #region Data members

        /// <summary>
        /// Holds the base register, if this is a relative memory access.
        /// </summary>
        private Register _base;

        /// <summary>
        /// Holds the address offset if used together with a base register or the absolute address, if register is null.
        /// </summary>
        private IntPtr _offset;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MemoryOperand"/>.
        /// </summary>
        /// <param name="type">The type of data held in the operand.</param>
        /// <param name="base">The base register, if this is an indirect access.</param>
        /// <param name="offset">The offset from the base register or absolute address to retrieve.</param>
        public MemoryOperand(SigType type, Register @base, IntPtr offset) :
            base(type)
        {
            _base = @base;
            _offset = offset;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the base register of this memory operand.
        /// </summary>
        public Register Base
        {
            get { return _base; }
        }

        /// <summary>
        /// Retrieves the offset from the base register, or the absolute address if base is null.
        /// </summary>
        public IntPtr Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion // Properties
    }
}
