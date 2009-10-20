/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 sti instruction.
    /// </summary>
    public sealed class StiInstruction : BaseInstruction
    {
        #region Methods

		/// <summary>
		/// Emits the specified CTX.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="emitter">The emitter.</param>
        public override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
			emitter.WriteByte(0xFB);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Sti(context);
		}

        #endregion // Methods
    }
}
