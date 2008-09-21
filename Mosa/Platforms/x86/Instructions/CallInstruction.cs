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
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// x86 specific intermediate representation of the call instruction.
    /// </summary>
    class CallInstruction : IR.IRInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the invocation target.
        /// </summary>
        private RuntimeMethod _invokeTarget;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CallInstruction"/> class.
        /// </summary>
        /// <param name="method">The method to call.</param>
        public CallInstruction(RuntimeMethod method) :
            base(0, 0)
        {
            if (null == method)
                throw new ArgumentNullException(@"method");

            _invokeTarget = method;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the invoke target.
        /// </summary>
        /// <value>The invoke target.</value>
        public RuntimeMethod InvokeTarget
        {
            get { return _invokeTarget; }
        }

        #endregion // Properties

        #region Methods

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

        #endregion // Methods
    }
}
