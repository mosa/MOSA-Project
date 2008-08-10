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

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Represents an x86 SSE2 register.
    /// </summary>
    /// <remarks>
    /// SSE2 is used by floating point instructions for their operands. An
    /// SSE2 register allows storage of double precision floating point values (64 bit)
    /// as required by the CIL standard.
    /// </remarks>
    public sealed class SSE2Register : GenericX86Register
    {
        #region Static data members

        /// <summary>
        /// Represents SSE2 register XMM0.
        /// </summary>
        public static readonly SSE2Register XMM0 = new SSE2Register(0);

        /// <summary>
        /// Represents SSE2 register XMM1.
        /// </summary>
        public static readonly SSE2Register XMM1 = new SSE2Register(1);

        /// <summary>
        /// Represents SSE2 register XMM2.
        /// </summary>
        public static readonly SSE2Register XMM2 = new SSE2Register(2);

        /// <summary>
        /// Represents SSE2 register XMM3.
        /// </summary>
        public static readonly SSE2Register XMM3 = new SSE2Register(3);

        /// <summary>
        /// Represents SSE2 register XMM4.
        /// </summary>
        public static readonly SSE2Register XMM4 = new SSE2Register(4);

        /// <summary>
        /// Represents SSE2 register XMM5.
        /// </summary>
        public static readonly SSE2Register XMM5 = new SSE2Register(5);

        /// <summary>
        /// Represents SSE2 register XMM6.
        /// </summary>
        public static readonly SSE2Register XMM6 = new SSE2Register(6);

        /// <summary>
        /// Represents SSE2 register XMM7.
        /// </summary>
        public static readonly SSE2Register XMM7 = new SSE2Register(7);

        #endregion // Static data members

        #region Data members

        /// <summary>
        /// Stores the register index of this instance.
        /// </summary>
        private int _registerCode;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new SSE2Register.
        /// </summary>
        /// <param name="registerCode">The SSE2 register index of this instance.</param>
        private SSE2Register(int registerCode)
        {
            _registerCode = registerCode;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Indicates that SSE2 registers are floating point register.
        /// </summary>
        public override bool IsFloatingPoint
        {
            get { return true; }
        }

        /// <summary>
        /// Returns the register index of this register.
        /// </summary>
        public override int RegisterCode
        {
            get { return _registerCode; }
        }

        /// <summary>
        /// Returns the width of the register.
        /// </summary>
        public override int Width
        {
            get { return 64; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Retrieves the SSE2 register name.
        /// </summary>
        /// <returns>The SSE2 register name.</returns>
        public override string ToString()
        {
            return String.Format("XMM#{0}", _registerCode);
        }

        #endregion // Methods
    }
}
