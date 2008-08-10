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
    /// Represents integral general purpose x86 registers.
    /// </summary>
    public sealed class GeneralPurposeRegister : GenericX86Register
    {
        #region Types

        /// <summary>
        /// Identifies x86 general purpose registers using their instruction encoding.
        /// </summary>
        private enum GPR
        {
            /// <summary>
            /// The x86 EAX register instruction encoding.
            /// </summary>
            EAX = 0,

            /// <summary>
            /// The x86 ECX register instruction encoding.
            /// </summary>
            ECX = 1,

            /// <summary>
            /// The x86 EDX register instruction encoding.
            /// </summary>
            EDX = 2,

            /// <summary>
            /// The x86 EBX register instruction encoding.
            /// </summary>
            EBX = 3,

            /// <summary>
            /// The x86 ESP register instruction encoding.
            /// </summary>
            ESP = 4,

            /// <summary>
            /// The x86 EBP register instruction encoding.
            /// </summary>
            EBP = 5,

            /// <summary>
            /// The x86 ESI register instruction encoding.
            /// </summary>
            ESI = 6,

            /// <summary>
            /// The x86 EDI register instruction encoding.
            /// </summary>
            EDI = 7
        }

        #endregion // Types

        #region Static data members

        /// <summary>
        /// Represents the EAX register.
        /// </summary>
        public static readonly GeneralPurposeRegister EAX = new GeneralPurposeRegister(GPR.EAX);

        /// <summary>
        /// Represents the ECX register.
        /// </summary>
        public static readonly GeneralPurposeRegister ECX = new GeneralPurposeRegister(GPR.ECX);

        /// <summary>
        /// Represents the EDX register.
        /// </summary>
        public static readonly GeneralPurposeRegister EDX = new GeneralPurposeRegister(GPR.EDX);

        /// <summary>
        /// Represents the EBX register.
        /// </summary>
        public static readonly GeneralPurposeRegister EBX = new GeneralPurposeRegister(GPR.EBX);

        /// <summary>
        /// Represents the ESP register.
        /// </summary>
        public static readonly GeneralPurposeRegister ESP = new GeneralPurposeRegister(GPR.ESP);

        /// <summary>
        /// Represents the EBP register.
        /// </summary>
        public static readonly GeneralPurposeRegister EBP = new GeneralPurposeRegister(GPR.EBP);

        /// <summary>
        /// Represents the ESI register.
        /// </summary>
        public static readonly GeneralPurposeRegister ESI = new GeneralPurposeRegister(GPR.ESI);

        /// <summary>
        /// Represents the EDI register.
        /// </summary>
        public static readonly GeneralPurposeRegister EDI = new GeneralPurposeRegister(GPR.EDI);

        #endregion // Static data members

        #region Data members

        /// <summary>
        /// Stores the general purpose register identified by this object instance.
        /// </summary>
        private GPR _gpr;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="GeneralPurposeRegister"/>.
        /// </summary>
        /// <param name="gpr">The general purpose register index.</param>
        private GeneralPurposeRegister(GPR gpr)
        {
            _gpr = gpr;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// General purpose register do not support floating point operations.
        /// </summary>
        public override bool IsFloatingPoint
        {
            get { return false; }
        }

        /// <summary>
        /// Returns the index of this register.
        /// </summary>
        public override int RegisterCode
        {
            get { return (int)_gpr; }
        }

        /// <summary>
        /// Returns the width of general purpose registers in bits.
        /// </summary>
        public override int Width
        {
            get { return 32; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Returns the name of the general purpose register.
        /// </summary>
        /// <returns>The name of the general purpose register.</returns>
        public override string ToString()
        {
            return _gpr.ToString();
        }

        #endregion // Methods
    }
}
