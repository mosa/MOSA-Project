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
    public abstract class LoadInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        protected LoadInstruction(OpCode code)
            : base(code, 0, 1)
        {
        }

        #endregion // Construction
    }
}
