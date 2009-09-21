/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Marks an IR instruction as being conditional.
    /// </summary>
    public interface IConditionalInstruction
    {
        /// <summary>
        /// Gets the condition code.
        /// </summary>
        /// <value>The condition code.</value>
        ConditionCode ConditionCode { get; set; }
    }
}
