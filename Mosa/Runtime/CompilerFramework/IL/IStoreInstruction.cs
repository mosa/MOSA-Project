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

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStoreInstruction
    {
        /// <summary>
        /// Gets the operands.
        /// </summary>
        /// <value>The operands.</value>
        Operand[] Operands { get; }
        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        Operand[] Results { get; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IStoreInstruction"/> is ignore.
        /// </summary>
        /// <value><c>true</c> if ignore; otherwise, <c>false</c>.</value>
        bool Ignore { get; set; }
    }
}
