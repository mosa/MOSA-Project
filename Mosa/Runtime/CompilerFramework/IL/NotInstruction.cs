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
    /// Intermediate representation of the IL unary not instruction, which calculates the bitwise
    /// complement of the input.
    /// </summary>
    public class NotInstruction : UnaryArithmeticInstruction
    {
        #region Operand Table

        /// <summary>
        /// Operand table according to ISO/IEC 23271:2006 (E), Partition III, 1.5, Table 5.
        /// </summary>
        private static readonly StackTypeCode[] _opTable = new StackTypeCode[] {
            StackTypeCode.Unknown,
            StackTypeCode.Int32,   
            StackTypeCode.Int64, 
            StackTypeCode.N,
            StackTypeCode.Unknown, 
            StackTypeCode.Unknown, 
            StackTypeCode.Unknown
        };

        #endregion // Operand Table

        #region Construction

        /// <summary>
		/// Initializes a new instance of <see cref="NegInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the instruction.</param>
		/// <exception cref="System.ArgumentException"><paramref name="code"/> is not OpCode.Neg.</exception>
		public NotInstruction(OpCode code)
			: base(code)
		{
			if (OpCode.Not != code)
				throw new ArgumentException(@"Invalid opcode.");
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
        
            // Validate the operand
            Operand[] ops = this.Operands;
            StackTypeCode result = _opTable[(int)ops[0].StackType];
            if (StackTypeCode.Unknown == result)
                throw new InvalidOperationException(@"Invalid operand to Neg instruction.");

            SetResult(0, compiler.CreateTemporary(ops[0].Type));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Not(this, arg);
        }

        #endregion // Methods
    }
}

