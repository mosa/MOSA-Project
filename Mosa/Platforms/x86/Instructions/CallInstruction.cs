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

namespace Mosa.Platforms.x86.Instructions
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

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Call(this, arg);
        }
    }
}
