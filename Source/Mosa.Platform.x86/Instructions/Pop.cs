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

		private static readonly LegacyOpCode POP = new LegacyOpCode(new byte[] { 0x8F }, 0);
		private static readonly LegacyOpCode POP_DS = new LegacyOpCode(new byte[] { 0x1F });
		private static readonly LegacyOpCode POP_ES = new LegacyOpCode(new byte[] { 0x07 });
		private static readonly LegacyOpCode POP_FS = new LegacyOpCode(new byte[] { 0x17 });
		private static readonly LegacyOpCode POP_GS = new LegacyOpCode(new byte[] { 0x0F, 0xA1 });
		private static readonly LegacyOpCode POP_SS = new LegacyOpCode(new byte[] { 0x0F, 0xA9 });

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
		internal override void EmitLegacy(InstructionNode node, X86CodeEmitter emitter)
		{
			if (node.Result.IsCPURegister)
			{
				if (node.Result.Register is SegmentRegister)
					switch ((node.Result.Register as SegmentRegister).Segment)
					{
						case SegmentRegister.SegmentType.DS: emitter.Emit(POP_DS); return;
						case SegmentRegister.SegmentType.ES: emitter.Emit(POP_ES); return;
						case SegmentRegister.SegmentType.FS: emitter.Emit(POP_FS); return;
						case SegmentRegister.SegmentType.GS: emitter.Emit(POP_GS); return;
						case SegmentRegister.SegmentType.SS: emitter.Emit(POP_SS); return;
						default: throw new InvalidOperationException(@"unable to emit opcode for segment register");
					}
				else
					emitter.WriteByte((byte)(0x58 + node.Result.Register.RegisterCode));
			}
			else
				emitter.Emit(POP, node.Result);
		}

		#endregion Methods
	}
}
