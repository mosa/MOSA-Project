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
    /// <summary>
    /// Intermediate representation of a IL binary logic instruction.
    /// </summary>
    public class BinaryLogicInstruction : BinaryInstruction
    {
        #region Operand Table

        /// <summary>
        /// Operand table according to ISO/IEC 23271:2006 (E), Partition III, 1.5, Table 5.
        /// </summary>
        private static readonly StackTypeCode[][] _opTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        #endregion // Operand Table

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="BinaryLogicInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the binary logic instruction.</param>
        public BinaryLogicInstruction(OpCode code)
            : base(code, 1)
        {
            // Check the opcode
            if (OpCode.And != code && OpCode.Div_un != code &&
                OpCode.Or != code && OpCode.Rem_un != code && OpCode.Xor != code)
                throw new ArgumentException(@"Invalid opcode passed to BinaryLogicInstruction.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(IMethodCompiler compiler)
        {
            base.Validate(compiler);
            Operand[] ops = this.Operands;
            StackTypeCode result = _opTable[(int)ops[0].StackType][(int)ops[1].StackType];
            if (StackTypeCode.Unknown == result)
                throw new ExecutionEngineException(@"Invalid stack result of instruction.");

            SetResult(0, compiler.CreateTemporary(Operand.SigTypeFromStackType(result)));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.BinaryLogic(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            string format;
            char op;
            bool unordered = false;

            switch (_code)
            {
                case OpCode.And:
                    op = '&';
                    break;

                case OpCode.Div_un:
                    op = '/';
                    unordered = true;
                    break;

                case OpCode.Or:
                    op = '|';
                    break;

                case OpCode.Rem_un:
                    op = '%';
                    unordered = true;
                    break;

                case OpCode.Xor:
                    op = '^';
                    break;

                default:
                    throw new InvalidOperationException();
            }

            if (false == unordered)
            {
                format = @"{0} = {1} {2} {3}";
            }
            else
            {
                format = @"{0} = unordered({1} {2} {3})";
            }

            Operand[] ops = this.Operands;
            return String.Format(format, this.Results[0], ops[0], op, ops[1]);
        }

        #endregion // Methods
    }
}
