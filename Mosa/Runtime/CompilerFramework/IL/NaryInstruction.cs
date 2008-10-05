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
    /// Represents an instruction, which takes an arbitrary number of stack operands.
    /// </summary>
    public abstract class NaryInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="NaryInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
        /// <param name="operandCount">The operand count.</param>
        protected NaryInstruction(OpCode code, int operandCount)
            : base(code, operandCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NaryInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
        /// <param name="operandCount">The operand count.</param>
        /// <param name="resultCount">The result count.</param>
        protected NaryInstruction(OpCode code, int operandCount, int resultCount)
            : base(code, operandCount, resultCount)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}", _code);
            Operand[] ops = this.Operands;
            if (0 != ops.Length)
            {
                builder.Append(' ');
                foreach (Operand op in ops)
                {
                    builder.AppendFormat("{0}, ", op);
                }
                builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }

        #endregion // Methods
    }
}
