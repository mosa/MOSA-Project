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
	/// Representations the x86 push instruction.
	/// </summary>
	public sealed class PushInstruction : BaseInstruction
	{
		#region Data Members

		private static readonly OpCode PUSH = new OpCode(new byte[] { 0xFF }, 6);
		private static readonly OpCode CONST8 = new OpCode(new byte[] { 0x6A });
		private static readonly OpCode CONST16 = new OpCode(new byte[] { 0x66, 0x68 });
		private static readonly OpCode CONST32 = new OpCode(new byte[] { 0x68 });
		private static readonly OpCode PUSH_CS = new OpCode(new byte[] { 0x0E });
		private static readonly OpCode PUSH_SS = new OpCode(new byte[] { 0x16 });
		private static readonly OpCode PUSH_DS = new OpCode(new byte[] { 0x1E });
		private static readonly OpCode PUSH_ES = new OpCode(new byte[] { 0x06 });
		private static readonly OpCode PUSH_FS = new OpCode(new byte[] { 0x0F, 0xA0 });
		private static readonly OpCode PUSH_GS = new OpCode(new byte[] { 0x0F, 0xA8 });


		#endregion

		#region Properties

		/// <summary>
		/// Gets the instruction latency.
		/// </summary>
		/// <value>The latency.</value>
		public override int Latency { get { return 3; } }

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			if (ctx.Operand1 is ConstantOperand) {
				if (IsByte(ctx.Result))
					emitter.Emit(CONST8, ctx.Operand1, null);
				else if (IsShort(ctx.Operand1) || IsChar(ctx.Operand1))
					emitter.Emit(CONST16, ctx.Operand1, null);
				else if (IsInt(ctx.Result))
					emitter.Emit(CONST32, ctx.Operand1, null);
			}
			else {
				if (ctx.Operand1 is RegisterOperand) {
					if ((ctx.Operand1 as RegisterOperand).Register is SegmentRegister)
						switch (((ctx.Operand1 as RegisterOperand).Register as SegmentRegister).Segment) {
							case SegmentRegister.SegmentType.CS: emitter.Emit(PUSH_CS, null, null); return;
							case SegmentRegister.SegmentType.SS: emitter.Emit(PUSH_SS, null, null); return;
							case SegmentRegister.SegmentType.DS: emitter.Emit(PUSH_DS, null, null); return;
							case SegmentRegister.SegmentType.ES: emitter.Emit(PUSH_ES, null, null); return;
							case SegmentRegister.SegmentType.FS: emitter.Emit(PUSH_FS, null, null); return;
							case SegmentRegister.SegmentType.GS: emitter.Emit(PUSH_GS, null, null); return;
							default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
						}
				}
				emitter.Emit(PUSH, ctx.Operand1, null);
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Push(context);
		}

		#endregion // Methods
	}
}
