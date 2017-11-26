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

		private static readonly LegacyOpCode PUSH = new LegacyOpCode(new byte[] { 0xFF }, 6);
		private static readonly LegacyOpCode CONST8 = new LegacyOpCode(new byte[] { 0x6A });
		private static readonly LegacyOpCode CONST16 = new LegacyOpCode(new byte[] { 0x66, 0x68 });
		private static readonly LegacyOpCode CONST32 = new LegacyOpCode(new byte[] { 0x68 });
		private static readonly LegacyOpCode PUSH_CS = new LegacyOpCode(new byte[] { 0x0E });
		private static readonly LegacyOpCode PUSH_SS = new LegacyOpCode(new byte[] { 0x16 });
		private static readonly LegacyOpCode PUSH_DS = new LegacyOpCode(new byte[] { 0x1E });
		private static readonly LegacyOpCode PUSH_ES = new LegacyOpCode(new byte[] { 0x06 });
		private static readonly LegacyOpCode PUSH_FS = new LegacyOpCode(new byte[] { 0x0F, 0xA0 });
		private static readonly LegacyOpCode PUSH_GS = new LegacyOpCode(new byte[] { 0x0F, 0xA8 });

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
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			if (node.Operand1.IsConstant)
			{
				if (node.Operand1.IsByte)
					emitter.Emit(CONST8, node.Operand1);
				else if (node.Operand1.IsShort || node.Operand1.IsChar)
					emitter.Emit(CONST16, node.Operand1);
				else if (node.Operand1.IsInt)
					emitter.Emit(CONST32, node.Operand1);
				return;
			}
			if (node.Operand1.IsCPURegister)
			{
				if (node.Operand1.Register is SegmentRegister)
				{
					switch ((node.Operand1.Register as SegmentRegister).Segment)
					{
						case SegmentRegister.SegmentType.CS: emitter.Emit(PUSH_CS); return;
						case SegmentRegister.SegmentType.SS: emitter.Emit(PUSH_SS); return;
						case SegmentRegister.SegmentType.DS: emitter.Emit(PUSH_DS); return;
						case SegmentRegister.SegmentType.ES: emitter.Emit(PUSH_ES); return;
						case SegmentRegister.SegmentType.FS: emitter.Emit(PUSH_FS); return;
						case SegmentRegister.SegmentType.GS: emitter.Emit(PUSH_GS); return;
						default: throw new InvalidOperationException("unable to emit opcode for segment register");
					}
				}
			}
			emitter.Emit(PUSH, node.Operand1);
		}

		#endregion Methods
	}
}
