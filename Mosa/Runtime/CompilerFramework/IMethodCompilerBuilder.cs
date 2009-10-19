/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;

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
