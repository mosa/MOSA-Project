/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Represents an arithmetic instruction.
    /// </summary>
    /// <summary>
    /// Instances of this class are used for add, div, mul,
    /// rem and sub instructions. The class checks that
    /// operands are valid combinations (according to ISO/IEC
    /// 23271:2006 (E), Partition III, §1.5, Table 2.)
    /// </summary>
    public abstract class ArithmeticInstruction : BinaryInstruction
    {
        #region Static data members

        /// <summary>
        /// Generic operand validation table. Not used for add and sub.
        /// </summary>
        private static StackTypeCode[][] _operandTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        /// <summary>
        /// Operand validation table for the add instruction.
        /// </summary>
        private static StackTypeCode[][] _addTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        /// <summary>
        /// Operand validation table for the sub instruction.
        /// </summary>
        private static StackTypeCode[][] _subTable = new StackTypeCode[][] {
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown },
            new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
        };

        #endregion // Static data members

        #region Construction

        /// <summary>
        /// Initializes a new instance <see cref="ArithmeticInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        public ArithmeticInstruction(OpCode code)
            : base(code, 1)
        {
            // Make sure the opcode is valid
            if (OpCode.Add != code && OpCode.Div != code && OpCode.Mul != code && OpCode.Rem != code && OpCode.Sub != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");
        }

        /// <summary>
        /// Initializes a new instance <see cref="ArithmeticInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        /// <param name="destination">The result operand.</param>
        /// <param name="source">The first source operand.</param>
        public ArithmeticInstruction(OpCode code, Operand destination, Operand source)
            : base(code, 1)
        {
            // Make sure the opcode is valid
            if (OpCode.Add != code && OpCode.Div != code && OpCode.Mul != code && OpCode.Rem != code && OpCode.Sub != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");

            // destination = destination op source
            SetResult(0, destination);
            SetOperand(0, destination);
            SetOperand(1, source);
        }

        /// <summary>
        /// Initializes a new instance <see cref="ArithmeticInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the arithmetic instruction to create.</param>
        /// <param name="destination">The result operand.</param>
        /// <param name="op1">The first source operand.</param>
        /// <param name="op2">The second source operand.</param>
        public ArithmeticInstruction(OpCode code, Operand destination, Operand op1, Operand op2)
            : base(code, 1)
        {
            // Make sure the opcode is valid
            if (OpCode.Add != code && OpCode.Div != code && OpCode.Mul != code && OpCode.Rem != code && OpCode.Sub != code)
                throw new ArgumentException(@"Opcode not supported.", @"code");

            // destination = destination op source
            SetResult(0, destination);
            SetOperand(0, op1);
            SetOperand(1, op2);
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Validates the instruction operands and creates a matching variable for the result.
        /// </summary>
        public sealed override void Validate(IMethodCompiler compiler)
        {
            base.Validate(compiler);

            Operand[] ops = this.Operands;
            StackTypeCode result = StackTypeCode.Unknown;
            switch (_code)
            {
                case OpCode.Add:
                    result = _addTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;

                case OpCode.Sub:
                    result = _subTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;

                default:
                    result = _operandTable[(int)ops[0].StackType][(int)ops[1].StackType];
                    break;
            }

            if (StackTypeCode.Unknown == result)
                throw new InvalidOperationException(@"Invalid operand types passed to " + _code);

            SigType resultType;
            if (result != StackTypeCode.Ptr)
            {
                resultType = Operand.SigTypeFromStackType(result);
            }
            else
            {
                // Copy the pointer element type
                PtrSigType op0 = ops[0].Type as PtrSigType;
                PtrSigType op1 = ops[1].Type as PtrSigType;
                if (op0 != null)
                    resultType = new PtrSigType(op0.CustomMods, op0.ElementType);
                else if (op1 != null)
                    resultType = new PtrSigType(op1.CustomMods, op1.ElementType);
                else
                    throw new InvalidOperationException();
            }

            SetResult(0, compiler.CreateTemporary(resultType));
        }

        #endregion // Methods
    }
}
