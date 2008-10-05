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
    /// Interface provided by method compiler builder stages.
    /// </summary>
    public interface IMethodCompilerBuilder
    {
        /// <summary>
        /// Gets the scheduled method builders.
        /// </summary>
        /// <value>A collection of the scheduled methods.</value>
        IEnumerable<MethodCompilerBase> Scheduled { get; }
    }
}
