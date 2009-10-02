using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of call context.
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
		/// Returns a string representation of the context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
        public override string ToString(Context context)
        {
			return String.Format(@"IR.call {0}", context.RuntimeField);
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
