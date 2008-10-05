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
    /// 
    /// </summary>
    public class EndfinallyInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EndfinallyInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public EndfinallyInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Endfinally == code);
            if (OpCode.Endfinally != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Endfinally(this, arg);
        }

        #endregion // Methods
    }
}
