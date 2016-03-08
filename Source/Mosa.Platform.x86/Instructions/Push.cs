// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 push instruction.
	/// </summary>
	public sealed class Push : X86Instruction
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

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Push"/>.
		/// </summary>
		public Push() :
			base(0, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		/// <exception cref="System.InvalidOperationException">@unable to emit opcode for segment register</exception>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			if (node.Operand1.IsConstant)
			{
				if (node.Operand1.IsByte)
					emitter.Emit(CONST8, node.Operand1, null);
				else if (node.Operand1.IsShort || node.Operand1.IsChar)
					emitter.Emit(CONST16, node.Operand1, null);
				else if (node.Operand1.IsInt)
					emitter.Emit(CONST32, node.Operand1, null);
				return;
			}
			if (node.Operand1.IsRegister)
			{
				if (node.Operand1.Register is SegmentRegister)
					switch ((node.Operand1.Register as SegmentRegister).Segment)
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
			emitter.Emit(PUSH, node.Operand1);
		}

		#endregion Methods
	}
}
