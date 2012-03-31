/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 push instruction.
	/// </summary>
	public sealed class PushInstruction : X86Instruction
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

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Operand1 is ConstantOperand)
			{
				if (IsByte(context.Operand1))
					emitter.Emit(CONST8, context.Operand1, null);
				else if (IsShort(context.Operand1) || IsChar(context.Operand1))
					emitter.Emit(CONST16, context.Operand1, null);
				else if (IsInt(context.Operand1))
					emitter.Emit(CONST32, context.Operand1, null);
                return;
			}
			if (context.Operand1 is RegisterOperand)
			{
				if ((context.Operand1 as RegisterOperand).Register is SegmentRegister)
					switch (((context.Operand1 as RegisterOperand).Register as SegmentRegister).Segment)
					{
						case SegmentRegister.SegmentType.CS: emitter.Emit(PUSH_CS, null, null); return;
						case SegmentRegister.SegmentType.SS: emitter.Emit(PUSH_SS, null, null); return;
						case SegmentRegister.SegmentType.DS: emitter.Emit(PUSH_DS, null, null); return;
						case SegmentRegister.SegmentType.ES: emitter.Emit(PUSH_ES, null, null); return;
						case SegmentRegister.SegmentType.FS: emitter.Emit(PUSH_FS, null, null); return;
						case SegmentRegister.SegmentType.GS: emitter.Emit(PUSH_GS, null, null); return;
						default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
					}
			}
			emitter.Emit(PUSH, context.Operand1, null, null);
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
