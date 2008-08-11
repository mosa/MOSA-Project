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
        /// Operand table according to ISO/IEC 23271:2006 (E), Partition III, §1.5, Table 5.
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

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture arch = methodCompiler.Architecture;
            switch (this.Code)
            {
                case OpCode.And:
                    return arch.CreateInstruction(typeof(IR.LogicalAndInstruction), this.Results[0], this.First, this.Second);

                default:
                    throw new NotImplementedException();
            }
        }

        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);
            Operand[] ops = this.Operands;
            StackTypeCode result = _opTable[(int)ops[0].StackType][(int)ops[1].StackType];
            if (StackTypeCode.Unknown == result)
                throw new ExecutionEngineException(@"Invalid stack result of instruction.");

            SetResult(0, CreateResultOperand(compiler.Architecture, result));
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.BinaryLogic(this);
        }

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
