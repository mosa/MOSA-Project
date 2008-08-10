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
using Mosa.Runtime.Metadata;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation for various IL call operations.
    /// </summary>
    /// <remarks>
    /// Instances of this class are used to represent call, calli and callvirt
    /// instructions.
    /// </remarks>
    public class CallInstruction : InvokeInstruction
    {
        #region Construction

        public CallInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Call == code);
        }

        #endregion // Construction

        #region Properties

        protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
        {
            get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
        }

        #endregion // Properties

        #region Methods

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Call(this);
        }

        #endregion // Methods
    }
}
