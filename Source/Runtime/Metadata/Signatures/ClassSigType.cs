/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// Represents a class type in a signature.
    /// </summary>
    public sealed class ClassSigType : TypeSigType
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassSigType"/> class.
        /// </summary>
        /// <param name="token">The class definition (a type definition, type reference or type specification) token.</param>
        public ClassSigType(TokenTypes token)
            : base(token, CilElementType.Class)
        {
        }

        #endregion // Construction

        #region SigType Overrides

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals(SigType other)
        {
            ClassSigType cst = other as ClassSigType;
            if (null == cst)
                return false;

            return (base.Equals(other) == true && this.Token == cst.Token);
        }

        #endregion // SigType Overrides

        /// <summary>
        /// Expresses the class reference in a meaningful, symbol-friendly string form
        /// </summary>
        public override string ToSymbolPart()
        {
            // FIXME: This needs to be a class name
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Token");
            //sb.AppendFormat("0x{0:X}", (int)this.Token);
            //return sb.ToString();
            throw new NotImplementedException();
        }
    }
}
