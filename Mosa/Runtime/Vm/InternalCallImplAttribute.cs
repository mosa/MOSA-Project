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

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// Used to identify the InternalCall to invoke as a replacement of the called method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public abstract class InternalCallImplAttribute : Attribute
    {
        /// <summary>
        /// Checks if this attribute matches the specified attribute.
        /// </summary>
        /// <param name="call">The call attribute.</param>
        /// <returns><c>true</c> if they match; otherwise <c>false</c>.</returns>
        public abstract bool Matches(InternalCallImplAttribute call);
    }
}
