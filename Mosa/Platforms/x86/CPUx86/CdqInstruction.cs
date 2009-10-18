/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Intermediate representation of the x86 cdq instruction.
    /// </summary>
    public sealed class CdqInstruction : BaseInstruction
    {
        #region Methods

        /// <summary>
        /// Emits the specified platform instruction.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="codeStream">The code stream.</param>
        public override void Emit(Context ctx, System.IO.Stream codeStream)
        {
            codeStream.WriteByte(0x99);
        }

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString(Context context)
        {
            return @"x86.cdq";
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cdq(context);
		}

        #endregion //  Methods
    }
}
