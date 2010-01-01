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
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 pop instruction.
	/// </summary>
	public sealed class PopInstruction : BaseInstruction
	{
		#region Data Members

		private static readonly OpCode POP = new OpCode(new byte[] { 0x8F });

		#endregion

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			if (ctx.Result is RegisterOperand) {
				if ((ctx.Result as RegisterOperand).Register is SegmentRegister)
					switch (((ctx.Result as RegisterOperand).Register as SegmentRegister).Segment) {
						case SegmentRegister.SegmentType.DS: emitter.WriteByte(0x1F); return;
						case SegmentRegister.SegmentType.ES: emitter.WriteByte(0x07); return;
						case SegmentRegister.SegmentType.FS: emitter.WriteByte(0x17); return;
						case SegmentRegister.SegmentType.GS: emitter.WriteByte(0x0F); emitter.WriteByte(0xA1); return;
						case SegmentRegister.SegmentType.SS: emitter.WriteByte(0x0F); emitter.WriteByte(0xA9); return;
						default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
					}
				else
					emitter.WriteByte((byte)(0x58 + (ctx.Result as RegisterOperand).Register.RegisterCode));
			}
			else
				emitter.Emit(POP.Code, 0, ctx.Result, null);
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
