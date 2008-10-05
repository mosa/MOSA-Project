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
    /// Pointer signature type.
    /// </summary>
    public sealed class PtrSigType : SigType
    {
        #region Data members

        /// <summary>
        /// Holds the modifiers of the pointer signature type.
        /// </summary>
        private CustomMod[] _customMods;

        /// <summary>
        /// Specifies the type pointed to.
        /// </summary>
        private SigType _elementType;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PtrSigType"/> class.
        /// </summary>
        /// <param name="customMods">The custom mods.</param>
        /// <param name="type">The type.</param>
        public PtrSigType(CustomMod[] customMods, SigType type)
            : base(CilElementType.Ptr)
        {
            if (null == type)
                throw new ArgumentNullException(@"type");

            _customMods = customMods;
            _elementType = type;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the custom modifiers of the pointer type.
        /// </summary>
        /// <value>The custom modifiers of the pointer type.</value>
        public CustomMod[] CustomMods { get { return _customMods; } }

        /// <summary>
        /// Gets the type pointed to.
        /// </summary>
        /// <value>The type pointed to.</value>
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
            PtrSigType pother = other as PtrSigType;
            if (null == pother)
                return false;

            return (base.Equals(other) == true && _elementType == pother._elementType && true == CustomMod.Equals(_customMods, pother._customMods));
        }

        #endregion // SigType Overrides
    }
}
