// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 pop instruction.
	/// </summary>
	public sealed class Pop : X86Instruction
	{
		#region Data Members

		private static readonly OpCode POP = new OpCode(new byte[] { 0x8F }, 0);
		private static readonly OpCode POP_DS = new OpCode(new byte[] { 0x1F });
		private static readonly OpCode POP_ES = new OpCode(new byte[] { 0x07 });
		private static readonly OpCode POP_FS = new OpCode(new byte[] { 0x17 });
		private static readonly OpCode POP_GS = new OpCode(new byte[] { 0x0F, 0xA1 });
		private static readonly OpCode POP_SS = new OpCode(new byte[] { 0x0F, 0xA9 });

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="X86Instruction"/> class.
		/// </summary>
		public Pop()
			: base(1, 0)
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
			if (node.Result.IsRegister)
			{
				if (node.Result.Register is SegmentRegister)
					switch ((node.Result.Register as SegmentRegister).Segment)
					{
						case SegmentRegister.SegmentType.DS: emitter.Emit(POP_DS, null, null); return;
						case SegmentRegister.SegmentType.ES: emitter.Emit(POP_ES, null, null); return;
						case SegmentRegister.SegmentType.FS: emitter.Emit(POP_FS, null, null); return;
						case SegmentRegister.SegmentType.GS: emitter.Emit(POP_GS, null, null); return;
						case SegmentRegister.SegmentType.SS: emitter.Emit(POP_SS, null, null); return;
						default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
					}
				else
					emitter.WriteByte((byte)(0x58 + node.Result.Register.RegisterCode));
			}
			else
				emitter.Emit(POP, node.Result, null);
		}

		#endregion Methods
	}
}
