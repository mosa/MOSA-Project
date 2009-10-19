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

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// Represents a value type in a signature.
    /// </summary>
    public sealed class ValueTypeSigType : SigType
    {
        #region Data members

        /// <summary>
        /// Holds the type definition, reference or specification token of the value type.
        /// </summary>
        private TokenTypes _token;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeSigType"/> class.
        /// </summary>
        /// <param name="token">The type definition, reference or specification token of the value type.</param>
        public ValueTypeSigType(TokenTypes token)
            : base(CilElementType.ValueType)
        {
            _token = token;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the value type token.
        /// </summary>
        /// <value>The token.</value>
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
            ValueTypeSigType vtst = other as ValueTypeSigType;
            if (vtst == null)
                return false;

            return (base.Equals(other) == true && _token == vtst._token);
        }

        #endregion // SigType Overrides
    }
}
