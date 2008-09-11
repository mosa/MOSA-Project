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
    /// 
    /// </summary>
    public interface IRuntimeAttributable
    {
        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <value>The custom attributes.</value>
        RuntimeAttribute[] CustomAttributes
        {
            get;
        }
    }
}
