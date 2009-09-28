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
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 setcc instruction.
    /// </summary>
    public sealed class SetccInstruction : OneOperandInstruction
    {

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SetccInstruction"/> class.
        /// </summary>
        public SetccInstruction()
        {
        }

        #endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 1; } }

		#endregion // Properties

        #region Methods

        /// <summary>
        /// Gets the condition code string.
        /// </summary>
        /// <returns>The string shortcut of the condition code.</returns>
        public static string GetConditionString(IR2.ConditionCode code)
        {
            string result;
            switch (code)
            {
                case IR2.ConditionCode.Equal: result = @"e"; break;
                case IR2.ConditionCode.GreaterOrEqual: result = @"ge"; break;
                case IR2.ConditionCode.GreaterThan: result = @"g"; break;
                case IR2.ConditionCode.LessOrEqual: result = @"le"; break;
                case IR2.ConditionCode.LessThan: result = @"l"; break;
                case IR2.ConditionCode.NotEqual: result = @"ne"; break;
                case IR2.ConditionCode.UnsignedGreaterOrEqual: result = @"ae"; break;
                case IR2.ConditionCode.UnsignedGreaterThan: result = @"a"; break;
                case IR2.ConditionCode.UnsignedLessOrEqual: result = @"be"; break;
                case IR2.ConditionCode.UnsignedLessThan: result = @"b"; break;
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
        public override string ToString(Context context)
        {
            return String.Format(@"x86 set{0} {1}", GetConditionString(context.ConditionCode), context.Operand1);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Setcc(context);
		}

        #endregion // OneOperandInstruction Overrides
    }
}
