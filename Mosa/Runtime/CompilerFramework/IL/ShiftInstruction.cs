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
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation for IL shift instructions.
    /// </summary>
    public class ShiftInstruction : BinaryInstruction
    {
        #region Operand Table

        /// <summary>
        /// This operand table conforms to ISO/IEC 23271:2006 (E), Partition III, §1.5, Table 6.
        /// </summary>
        private static readonly StackTypeCode[][] _operandTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        #endregion // Operand Table

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ShiftInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the shift instruction.</param>
        public ShiftInstruction(OpCode code)
            : base(code, 1)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Validates the operands and creates a result
        /// </summary>
        public sealed override void Validate(MethodCompilerBase compiler)
        {
            Operand[] ops = this.Operands;
            StackTypeCode result = _operandTable[(int)ops[0].StackType][(int)ops[1].StackType];
            Debug.Assert(StackTypeCode.Unknown != result, @"Can't shift with the given stack operands.");
            if (StackTypeCode.Unknown == result)
                throw new ExecutionEngineException(@"Invalid stack state.");

            SetResult(0, compiler.CreateResultOperand(Operand.SigTypeFromStackType(result)));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Shift(this, arg);
        }

        #endregion // Methods
    }
}
