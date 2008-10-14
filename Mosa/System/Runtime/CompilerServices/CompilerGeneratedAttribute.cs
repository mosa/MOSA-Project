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

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Distinguishes a compiler-generated element from a user-generated element. This class cannot be inherited.
    /// </summary>
    public sealed class CompilerGeneratedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerGeneratedAttribute"/> class.
        /// </summary>
        public CompilerGeneratedAttribute()
        {
        }
    }
}
