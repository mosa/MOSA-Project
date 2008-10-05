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
    /// Represents a generic parameter type of a generic member definition.
    /// </summary>
    public sealed class MVarSigType : SigType
    {
        #region Data members

        /// <summary>
        /// Holds the index of the generic parameter in the generic parameter list.
        /// </summary>
        private int _index;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MVarSigType"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public MVarSigType(int index) : 
            base(CilElementType.MVar)
        {
            _index = index;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the index of the generic parameter in the generic parameter list of the generic member.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get { return _index; } }

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
            MVarSigType mvst = other as MVarSigType;
            if (mvst == null)
                return false;

            return (base.Equals(other) == true && _index == mvst._index);
        }

        #endregion // SigType Overrides
    }
}
