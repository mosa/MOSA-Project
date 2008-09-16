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

namespace Mosa.Runtime.Metadata {
    /// <summary>
    /// 
    /// </summary>
	public enum HeapType {
        /// <summary>
        /// 
        /// </summary>
		String = 0,
        /// <summary>
        /// 
        /// </summary>
		UserString = 1,
        /// <summary>
        /// 
        /// </summary>
		Blob = 2,
        /// <summary>
        /// 
        /// </summary>
		Guid = 3,
        /// <summary>
        /// 
        /// </summary>
		Tables = 4,
        /// <summary>
        /// 
        /// </summary>
		MaxType = 5
	}
}
