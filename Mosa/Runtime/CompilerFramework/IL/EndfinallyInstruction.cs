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
    public class EndfinallyInstruction : ILInstruction
    {
        #region Construction

        public EndfinallyInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Endfinally == code);
            if (OpCode.Endfinally != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Endfinally(this);
        }

        #endregion // Methods
    }
}
