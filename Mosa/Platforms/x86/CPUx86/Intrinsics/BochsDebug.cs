using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using IR2 = Mosa.Runtime.CompilerFramework.IR2;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 CPUID instruction.
    /// </summary>
    public sealed class BochsDebug : BaseInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CpuIdInstruction"/> class.
        /// </summary>
        public BochsDebug() :
            base()
        {
        }

        #endregion // Construction

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 xchg bx, bx");
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.BochsDebug(context);
		}

    }
}
