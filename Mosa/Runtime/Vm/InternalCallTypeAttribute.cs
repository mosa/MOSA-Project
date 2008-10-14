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
    /// Marks types providing implementations for internal calls.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class InternalCallTypeAttribute : Attribute
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalCallTypeAttribute"/> class.
        /// </summary>
        public InternalCallTypeAttribute()
        {
        }

        #endregion // Construction
    }
}
