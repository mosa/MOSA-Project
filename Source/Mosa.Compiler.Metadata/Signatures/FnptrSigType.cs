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
    /// Specifies a function pointer signature type.
    /// </summary>
    public sealed class FnptrSigType : SigType
    {
        #region Data members

        /// <summary>
        /// The token type of a MethodDef, MethodRef or MethodSpec row, that holds the function signature.
        /// </summary>
        private HeapIndexToken _token;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FnptrSigType"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public FnptrSigType(HeapIndexToken token) :
            base(CilElementType.FunctionPtr)
        {
            _token = token;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the method definition, reference or specification token.
        /// </summary>
        /// <value>The token.</value>
        public HeapIndexToken Token { get { return _token; } }

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
            FnptrSigType fst = other as FnptrSigType;
            if (null == fst)
                return false;

            return (base.Equals(other) == true && _token == fst._token);
        }

        #endregion // SigType Overrides
    }
}
