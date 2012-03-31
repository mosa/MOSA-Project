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
	/// Representations the x86 pop instruction.
	/// </summary>
	public sealed class PopInstruction : X86Instruction
	{
		#region Data Members

		private static readonly OpCode POP = new OpCode(new byte[] { 0x8F }, 0);
		private static readonly OpCode POP_DS = new OpCode(new byte[] { 0x1F });
		private static readonly OpCode POP_ES = new OpCode(new byte[] { 0x07 });
		private static readonly OpCode POP_FS = new OpCode(new byte[] { 0x17 });
		private static readonly OpCode POP_GS = new OpCode(new byte[] { 0x0F, 0xA1 });
		private static readonly OpCode POP_SS = new OpCode(new byte[] { 0x0F, 0xA9 });

		#endregion

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context context, MachineCodeEmitter emitter)
		{
			if (context.Result is RegisterOperand)
			{
				if ((context.Result as RegisterOperand).Register is SegmentRegister)
					switch (((context.Result as RegisterOperand).Register as SegmentRegister).Segment)
					{
						case SegmentRegister.SegmentType.DS: emitter.Emit(POP_DS, null, null); return;
						case SegmentRegister.SegmentType.ES: emitter.Emit(POP_ES, null, null); return;
						case SegmentRegister.SegmentType.FS: emitter.Emit(POP_FS, null, null); return;
						case SegmentRegister.SegmentType.GS: emitter.Emit(POP_GS, null, null); return;
						case SegmentRegister.SegmentType.SS: emitter.Emit(POP_SS, null, null); return;
						default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
					}
				else
					emitter.WriteByte((byte)(0x58 + (context.Result as RegisterOperand).Register.RegisterCode));
			}
			else
				emitter.Emit(POP, context.Result, null);
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
