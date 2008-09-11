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

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMethodCompilerBuilder
    {
        /// <summary>
        /// Gets the scheduled.
        /// </summary>
        /// <value>The scheduled.</value>
        IEnumerable<MethodCompilerBase> Scheduled { get; }
    }
}
