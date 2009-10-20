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

namespace Mosa.Platforms.x86.CPUx86.Intrinsics
{
	/// <summary>
	/// Intermediate representation of the x86 pop instruction.
	/// </summary>
	public sealed class PopInstruction : OneOperandInstruction
	{
		#region Data Members

		private static readonly OpCode POP = new OpCode(new byte[] { 0x8F });

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PopInstruction"/> class.
		/// </summary>
		public PopInstruction() :
			base()
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			if (ctx.Operand1 is RegisterOperand)
				emitter.WriteByte((byte)(0x58 + (ctx.Operand1 as RegisterOperand).Register.RegisterCode));
			else
				emitter.Emit(POP.Code, 0, ctx.Operand1, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Pop(context);
		}

		#endregion // Methods
	}
}
