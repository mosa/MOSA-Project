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
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the IL throw instruction.
    /// </summary>
    public class ThrowInstruction : UnaryInstruction
    {
        #region Construction

        public ThrowInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Throw == code);
        }

        #endregion // Construction

        #region Properties

        public sealed override FlowControl FlowControl
        {
            get { return FlowControl.Throw; }
        }

        #endregion // Properties

        #region Methods

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Throw(this);
        }

        #endregion // Methods
    }
}
