/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// Represents a value type in a signature.
    /// </summary>
    public sealed class ValueTypeSigType : TypeSigType
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeSigType"/> class.
        /// </summary>
        /// <param name="token">The type definition, reference or specification token of the value type.</param>
        public ValueTypeSigType(TokenTypes token) 
			: base(token, CilElementType.ValueType)
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
            ValueTypeSigType vtst = other as ValueTypeSigType;
            if (vtst == null)
                return false;

            return (base.Equals(other) == true && this.Token == vtst.Token);
        }

        #endregion // SigType Overrides
    }
}
