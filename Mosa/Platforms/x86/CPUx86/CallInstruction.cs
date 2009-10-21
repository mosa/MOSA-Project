/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using System.Diagnostics;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 call instruction.
	/// </summary>
	public sealed class CallInstruction : OneOperandInstruction
	{
		
		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			emitter.WriteByte(0xE8);
			emitter.Write(new byte[4], 0, 4);

			//long address = _linker.Link(
			//    LinkType.RelativeOffset | LinkType.I4,
			//    _compiler.Method,
			//    (int)(_codeStream.Position - _codeStreamBasePosition) - 4,
			//    (int)(_codeStream.Position - _codeStreamBasePosition),
			//    target,
			//    IntPtr.Zero
			//);

		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Call(context);
		}

		#endregion // Methods
	}
}
