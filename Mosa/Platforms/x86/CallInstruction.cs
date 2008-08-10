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
using IL = Mosa.Runtime.CompilerFramework.IL;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86
{
    class CallInstruction : IL.CallInstruction, IRegisterConstraint
    {
        #region Construction

        public CallInstruction(IL.OpCode code) :
            base(code)
        {
        }

        #endregion // Construction

        #region IRegisterConstraint Members

        Register[] IRegisterConstraint.GetConstraints()
        {
            return null;
        }

        Register[] IRegisterConstraint.GetRegistersUsed()
        {
            return null;
        }

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            // TODO
            return this;
        }

        #endregion // IRegisterConstraint Members
    }
}
