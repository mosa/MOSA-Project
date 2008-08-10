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

        public override void Visit(IILVisitor visitor)
        {
            visitor.UnaryArithmetic(this);
        }

        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);

            // Simple result is the same type as the unary argument
            SetResult(0, CreateResultOperand(compiler.Architecture, this.Operands[0].Type));
        }

        #endregion // Methods
    }
}
