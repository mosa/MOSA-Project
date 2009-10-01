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

using Mosa.Runtime.CompilerFramework;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the x86 setcc instruction.
    /// </summary>
    public sealed class SetccInstruction : IR.OneOperandInstruction
    {
        #region Data members

        /// <summary>
        /// Holds the condition code of this instruction.
        /// </summary>
        private IR.ConditionCode _code;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SetccInstruction"/> class.
        /// </summary>
        public SetccInstruction()
        {
            _code = IR.ConditionCode.Equal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetccInstruction"/> class.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="code">The code.</param>
        public SetccInstruction(Operand destination, IR.ConditionCode code) :
            base(destination)
        {
            _code = code;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the condition code.
        /// </summary>
        /// <value>The condition code.</value>
        public IR.ConditionCode ConditionCode
        {
            get { return _code; }
            set { _code = value; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Gets the condition code string.
        /// </summary>
        /// <returns>The string shortcut of the condition code.</returns>
        public static string GetConditionString(IR.ConditionCode code)
        {
            string result;
            switch (code)
            {
                case IR.ConditionCode.Equal: result = @"e"; break;
                case IR.ConditionCode.GreaterOrEqual: result = @"ge"; break;
                case IR.ConditionCode.GreaterThan: result = @"g"; break;
                case IR.ConditionCode.LessOrEqual: result = @"le"; break;
                case IR.ConditionCode.LessThan: result = @"l"; break;
                case IR.ConditionCode.NotEqual: result = @"ne"; break;
                case IR.ConditionCode.UnsignedGreaterOrEqual: result = @"ae"; break;
                case IR.ConditionCode.UnsignedGreaterThan: result = @"a"; break;
                case IR.ConditionCode.UnsignedLessOrEqual: result = @"be"; break;
                case IR.ConditionCode.UnsignedLessThan: result = @"b"; break;
                default:
                    throw new NotSupportedException();
            }
            return result;
        }

        #endregion // Methods

        #region OneOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 set{0} {1}", GetConditionString(_code), this.Operand0);
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Setcc(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // OneOperandInstruction Overrides
    }
}
