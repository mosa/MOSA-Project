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
    /// <summary>
    /// Represents a unary instruction, which performs an operation on the operand and places
    /// the result on the stack.
    /// </summary>
    public class UnaryArithmeticInstruction : UnaryInstruction
    {
        #region Construction

		/// <summary>
        /// Initializes a new instance of <see cref="UnaryArithmeticInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the instruction.</param>
		public UnaryArithmeticInstruction(OpCode code)
			: base(code, 1)
		{
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.UnaryArithmetic(this, arg);
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(IMethodCompiler compiler)
        {
            base.Validate(compiler);

            // Simple result is the same type as the unary argument
            SetResult(0, compiler.CreateTemporary(this.Operands[0].Type));
        }

        #endregion // Methods
    }
}
