/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata
{
    /// <summary>
    /// 
    /// </summary>
    public enum ManifestResourceAttributes
    {
        /// <summary>
        /// 
        /// </summary>
        VisibilityMask = 0x0007,
        /// <summary>
        /// 
        /// </summary>
        Public = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        Private = 0x0002
    }
}
