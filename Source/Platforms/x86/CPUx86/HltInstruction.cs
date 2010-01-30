/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.CPUx86
{
    /// <summary>
    /// Representations the x86 hlt instruction.
    /// </summary>
    public sealed class HltInstruction : BaseInstruction
    {
        #region Methods

		/// <summary>
		/// Emits the specified CTX.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
        {
			emitter.WriteByte(0xF4);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Hlt(context);
		}

        #endregion // Methods
    }
}
