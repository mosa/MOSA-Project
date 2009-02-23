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

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// Specifies an instance of a generic type as a signature type.
    /// </summary>
    public sealed class GenericInstSigType : SigType
    {
        #region Data members

        /// <summary>
        /// The generic type of the signature type.
        /// </summary>
        private SigType _baseType;

        /// <summary>
        /// Array of generic argument types to specify the generic type.
        /// </summary>
        private SigType[] _genericArgs;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericInstSigType"/> class.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="genericArgs">The generic args.</param>
        public GenericInstSigType(SigType baseType, SigType[] genericArgs) : 
            base(CilElementType.GenericInst)
        {
            _baseType = baseType;
            _genericArgs = genericArgs;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the generic type of this signature type.
        /// </summary>
        /// <value>The type of the generic type.</value>
        public SigType BaseType { get { return _baseType; } }

        /// <summary>
        /// Gets the generic parameter type signatures.
        /// </summary>
        /// <value>The generic type signatures.</value>
        public SigType[] GenericArgs { get { return _genericArgs; } }

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
            GenericInstSigType gist = other as GenericInstSigType;
            if (null == gist)
                return false;

            return (true == base.Equals(other) && _baseType == gist._baseType && true == SigType.Equals(_genericArgs, gist._genericArgs));
        }

        #endregion // SigType Overrides

        /// <summary>
        /// Expresses the generic instance type in a meaningful, symbol-friendly string form
        /// </summary>
        public override string ToSymbolPart()
        {
            StringBuilder sb = new StringBuilder();
            SigType[] genericArgs = this.GenericArgs;
            sb.Append(this.BaseType.ToSymbolPart());
            sb.Append('<');
            for (int x = 0; x < genericArgs.GetLength(0); x++)
            {
                if (x > 0)
                    sb.Append(',');
                sb.Append(genericArgs[x].ToSymbolPart());
            }
            sb.Append('>');
            return sb.ToString();
        }
    }
}
