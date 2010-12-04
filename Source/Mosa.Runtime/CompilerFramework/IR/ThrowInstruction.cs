using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ThrowInstruction : OneOperandInstruction
    {
        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
            visitor.ThrowInstruction(context);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString(Context context)
        {
            return String.Format(@"IR throw {0}", context.Operand1);
        }
    }
}
