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
    public abstract class LoadInstruction : ILInstruction
    {
        #region Construction

        protected LoadInstruction(OpCode code)
            : base(code, 0, 1)
        {
        }

        #endregion // Construction
    }
}
