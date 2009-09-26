using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of call instruction.
    /// </summary>
    public class CallInstruction : BaseInstruction
    {
        #region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallInstruction"/> class.
		/// </summary>
        public CallInstruction()
        {
        }

        #endregion // Construction

        #region IRInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(ref InstructionData instruction)
        {
			return String.Format(@"IR.call {0}", instruction.RuntimeField);
        }

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.CallInstruction(context);
        }

        #endregion // IRInstruction Overrides
    }
}
