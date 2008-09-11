using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Abstract base class for all instructions in the intermediate representation.
    /// </summary>
    public abstract class IRInstruction : Instruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="IRInstruction"/>.
        /// </summary>
        protected IRInstruction() :
            base()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IRInstruction"/>.
        /// </summary>
        /// <param name="operandCount">Specifies the number of operands of the instruction.</param>
        protected IRInstruction(int operandCount) :
            base(operandCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IRInstruction"/>.
        /// </summary>
        /// <param name="operandCount">Specifies the number of operands of the instruction.</param>
        /// <param name="resultCount">Specifies the number of results of the instruction.</param>
        protected IRInstruction(int operandCount, int resultCount) :
            base(operandCount, resultCount)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>A string representation of the instruction in intermediate form.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Determines if the visitor supports IIRVisitor and dispatches visitation appropriately.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IIRVisitor<ArgType> irv = visitor as IIRVisitor<ArgType>;
            if (null != irv)
                Visit(irv, arg);
            else
                base.Visit(visitor, arg);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected abstract void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg);

        #endregion // Methods
    }
}
