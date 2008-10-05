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
    /// Intermediate representation of an instruction, which takes two stack arguments.
    /// </summary>
    public abstract class BinaryInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="BinaryInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the binary instruction.</param>
        protected BinaryInstruction(OpCode code)
            : base(code, 2)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BinaryInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the binary instruction.</param>
        /// <param name="resultCount">The result count.</param>
        protected BinaryInstruction(OpCode code, int resultCount)
            : base(code, 2, resultCount)
        {
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the first operand of the binary instruction.
        /// </summary>
        public Operand First
        {
            get { return this.Operands[0]; }
            set 
            {
                Operand[] ops = this.Operands;
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (null != ops[0] && value.Type != ops[0].Type)
                    throw new ArgumentException(@"Operand type is not mutable.", @"value");

                ops[0] = value; 
            }
        }

        /// <summary>
        /// Retrieves the second operand of the binary instruction.
        /// </summary>
        public Operand Second
        {
            get { return this.Operands[1]; }
            set
            {
                Operand[] ops = this.Operands;
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (null != ops[1] && value.Type != ops[1].Type)
                    throw new ArgumentException(@"Operand type is not mutable.", @"value");

                ops[1] = value;
            }
        }

        #endregion // Properties
    }
}
