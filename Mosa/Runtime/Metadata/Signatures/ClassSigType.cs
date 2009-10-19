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
    public sealed class ClassSigType : SigType
    {
        #region Data members

        /// <summary>
        /// The token of the class type in the signature.
        /// </summary>
        private TokenTypes _token;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassSigType"/> class.
        /// </summary>
        /// <param name="token">The class definition (a type definition, type reference or type specification) token.</param>
        public ClassSigType(TokenTypes token)
            : base(CilElementType.Class)
        {
            _token = token;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type definition, reference or specification token.
        /// </summary>
        /// <value>The token of the class.</value>
        public TokenTypes Token { get { return _token; } }

        #endregion // Properties

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

            return (base.Equals(other) == true && _token == cst._token);
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
