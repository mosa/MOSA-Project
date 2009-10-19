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

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// 
    /// </summary>
    [Flags]
	public enum ParameterAttributes : ushort {
        /// <summary>
        /// 
        /// </summary>
		None = 0x0000,
        /// <summary>
        /// 
        /// </summary>
		In = 0x0001,
        /// <summary>
        /// 
        /// </summary>
		Out = 0x0002,
        /// <summary>
        /// 
        /// </summary>
		Optional = 0x0010,
        /// <summary>
        /// 
        /// </summary>
		HasDefault = 0x1000,
        /// <summary>
        /// 
        /// </summary>
		HasFieldMarshal = 0x2000,
        /// <summary>
        /// 
        /// </summary>
		Unused = 0xcfe0
	}
}
