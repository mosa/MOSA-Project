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
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86
{
    class CallInstruction : Instruction
    {
        #region Data members

        private RuntimeMethod _invokeTarget;

        #endregion // Data members

        #region Construction

        public CallInstruction(RuntimeMethod method) :
            base(0, 0)
        {
            if (null == method)
                throw new ArgumentNullException(@"method");

            _invokeTarget = method;
        }

        #endregion // Construction

        #region Properties

        public RuntimeMethod InvokeTarget
        {
            get { return _invokeTarget; }
        }

        #endregion // Properties

        public override void Visit(IInstructionVisitor visitor)
        {
            IX86InstructionVisitor x86v = visitor as IX86InstructionVisitor;
            if (null == x86v)
                throw new ArgumentException(@"Visitor doesn't implement IX86InstructionVisitor.");

            x86v.Call(this);
        }
    }
}
