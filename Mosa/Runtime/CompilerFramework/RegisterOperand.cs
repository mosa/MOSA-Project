/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Represents an operand stored in a machine specific register.
    /// </summary>
    public class RegisterOperand : Operand
    {
        #region Data members

        /// <summary>
        /// The register, where the operand is stored.
        /// </summary>
        private Register _register;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new <see cref="RegisterOperand"/>.
        /// </summary>
        /// <param name="type">The signature type of the value the register holds.</param>
        /// <param name="register">The machine specific register used.</param>
        public RegisterOperand(SigType type, Register register) :
            base(type)
        {
            if (null == register)
                throw new ArgumentNullException(@"register");

            _register = register;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// This is a register operand.
        /// </summary>
        public override bool IsRegister
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves the register, where the operand is located.
        /// </summary>
        public Register Register
        {
            get { return _register; }
        }

        #endregion // Properties

        #region Operand Overrides

        /// <summary>
        /// Compares with the given operand for equality.
        /// </summary>
        /// <param name="other">The other operand to compare with.</param>
        /// <returns>The return value is true if the operands are equal; false if not.</returns>
        public override bool Equals(Operand other)
        {
            RegisterOperand rop = other as RegisterOperand;
            return (null != rop && ReferenceEquals(rop.Register, Register));
        }

        /// <summary>
        /// Returns a string representation of <see cref="RegisterOperand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
        public override string ToString()
        {
            return String.Format("{0} {1}", _register, base.ToString());
        }

        #endregion // Operand Overrides
    }
}
