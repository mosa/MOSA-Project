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
    /// Reference signature type.
    /// </summary>
    public sealed class RefSigType : SigType
    {
        #region Data members

        /// <summary>
        /// The type referenced by this signature type.
        /// </summary>
        private SigType _elementType;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RefSigType"/> class.
        /// </summary>
        /// <param name="type">The referenced type.</param>
        public RefSigType(SigType type) : 
            base(CilElementType.ByRef)
        {
            if (null == type)
                throw new ArgumentNullException(@"type");

            _elementType = type;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type referenced by this signature type.
        /// </summary>
        /// <value>The referenced type.</value>
        public SigType ElementType { get { return _elementType; } }

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
            RefSigType rst = other as RefSigType;
            if (null == rst)
                return false;

            return (true == base.Equals(other) && _elementType == rst._elementType);
        }

        #endregion // SigType Overrides
    }
}
