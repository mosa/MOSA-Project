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
    /// Implements the internal validating representation for the CIL neg instruction.
    /// </summary>
    public class NegInstruction : UnaryArithmeticInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the typecode validation table from ISO/IEC 23271:2006 (E),
        /// Partition III, §1.5, Table 3.
        /// </summary>
        private static StackTypeCode[] _typeCodes = new StackTypeCode[] {
			StackTypeCode.Unknown,
			StackTypeCode.Int32,
			StackTypeCode.Int64,
			StackTypeCode.N,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown,
			StackTypeCode.Unknown
		};

        /// <summary>
        /// Operand, which represents the result of the operation.
        /// </summary>
        private Operand _result;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="NegInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the instruction.</param>
        /// <exception cref="System.ArgumentException"><paramref name="code"/> is not OpCode.Neg.</exception>
        public NegInstruction(OpCode code)
            : base(code)
        {
            if (OpCode.Neg != code)
                throw new ArgumentException(@"Invalid opcode.");
        }

        #endregion // Construction

        #region Properties

        public Operand Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (value.Type != _result.Type)
                    throw new ArgumentException(@"Operand type is not mutable.", @"value");

                _result = value;
            }
        }

        #endregion // Properties

        #region Methods

        /*
        public override Operand Fold()
        {
            Constant op = (Constant)_unary;
            object result;
            switch (op.StackType)
            {
                case StackTypeCode.Int32:
                    int i = (int)op.Value;
                    result = -i;
                    break;

                case StackTypeCode.Int64:
                    long l = (long)op.Value;
                    result = -l;
                    break;

                case StackTypeCode.N:
                    int n = (int)op.Value;
                    result = -n;
                    break;

                case StackTypeCode.F:
                    double f = (double)op.Value;
                    result = -f;
                    break;

                default:
                    throw new InvalidOperationException(@"Negation not supported for unary argument type.");
            }
            return new Constant(_result.Type, result);
        }
*/
        public override void Validate(MethodCompilerBase compiler)
        {
            base.Validate(compiler);

            Operand op = this.Operands[0];

            // Validate the operand
            StackTypeCode result = _typeCodes[(int)op.StackType];
            if (StackTypeCode.Unknown == result)
                throw new InvalidOperationException(@"Invalid operand to Neg instruction.");

            SetResult(0, compiler.CreateResultOperand(op.Type));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Neg(this, arg);
        }

        #endregion // Methods
    }
}