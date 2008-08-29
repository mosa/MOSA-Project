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

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class ArithmeticOverflowInstruction : BinaryInstruction
    {
        #region Static data members

        /// <summary>
        /// Generic operand validation table. Not used for add and sub.
        /// </summary>
        private static StackTypeCode[][] _operandTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        /// <summary>
        /// Operand validation table for the add instruction.
        /// </summary>
        private static StackTypeCode[][] _addovfunTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        /// <summary>
        /// Operand validation table for the sub instruction.
        /// </summary>
        private static StackTypeCode[][] _subovfunTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        #endregion // Static data members

        #region Construction

        /// <summary>
        /// Initializes a new instance <see cref="ArithmeticInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        public ArithmeticOverflowInstruction(OpCode code)
            : base(code, 1)
        {
            // Make sure the opcode is valid
            if (OpCode.Add_ovf != code && OpCode.Add_ovf_un != code && 
                OpCode.Mul_ovf != code && OpCode.Mul_ovf_un != code && 
                OpCode.Sub_ovf != code && OpCode.Sub_ovf_un != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Validates the instruction operands and creates a matching variable for the result.
        /// </summary>
        public sealed override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);

            Operand[] ops = this.Operands;
            StackTypeCode result = StackTypeCode.Unknown;
            switch (_code)
            {
                case OpCode.Add_ovf_un:
                    result = _addovfunTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;

                case OpCode.Sub_ovf_un:
                    result = _subovfunTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;

                default:
                    result = _operandTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;
            }

            if (StackTypeCode.Unknown == result)
                throw new InvalidOperationException(@"Invalid operand types passed to " + _code);

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
            visitor.ArithmeticOverflow(this, arg);
        }

        #endregion // Methods
    }
}
