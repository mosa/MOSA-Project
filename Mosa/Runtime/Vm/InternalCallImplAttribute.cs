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

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// Used to identify the InternalCall to invoke as a replacement of the called method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public abstract class InternalCallImplAttribute : Attribute
    {
        #region Methods

        /// <summary>
        /// Checks if this attribute matches the specified attribute.
        /// </summary>
        /// <param name="call">The call attribute.</param>
        /// <returns><c>true</c> if they match; otherwise <c>false</c>.</returns>
        public abstract bool Match(InternalCallImplAttribute call);

        #endregion // Methods

        #region Attribute Overrides

        /// <summary>
        /// When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this instance of <see cref="T:System.Attribute"/>.</param>
        /// <returns>
        /// true if this instance equals <paramref name="obj"/>; otherwise, false.
        /// </returns>
        public override bool Match(object obj)
        {
            InternalCallImplAttribute icia = obj as InternalCallImplAttribute;
            if (icia != null)
                return Match(icia);

            return false;
        }

        #endregion // Attribute Overrides
    }
}
