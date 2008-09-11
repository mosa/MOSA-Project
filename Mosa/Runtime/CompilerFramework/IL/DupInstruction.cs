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
    public class DupInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DupInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public DupInstruction(OpCode code)
            : base(code, 2)
        {
            Debug.Assert(OpCode.Dup == code);
            if (OpCode.Dup != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);
            Operand[] ops = this.Operands;
            SetResult(0, ops[0]);
            SetResult(1, ops[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Dup(this, arg);
        }

        #endregion // Methods
    }
}
