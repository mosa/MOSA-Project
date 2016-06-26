// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Movzx instruction.
	/// </summary>
	public sealed class Movzx : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Movzx" />.
		/// </summary>
		public Movzx() :
			base(1, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			MovzxRegToReg(node, emitter);
		}

		private static void MovzxRegToReg(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);
			Debug.Assert(node.Operand1.IsRegister);

			var size = BaseMethodCompilerStage.GetInstructionSize(node.Size, node.Result);

			// register2 to register1 0000 1111 : 1011 011w : 11 reg1 reg2
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                           // 4:opcode
				.AppendNibble(Bits.b1111)                           // 4:opcode
				.AppendNibble(Bits.b1011)                           // 4:opcode
				.Append3Bits(Bits.b011)                             // 4:opcode
				.AppendWidthBit(size != InstructionSize.Size8)      // 1:width
				.AppendMod(Bits.b11)                                // 2:mod
				.AppendRegister(node.Result)                        // 3:register (destination)
				.AppendRM(node.Operand1);                           // 3:r/m (source)

			emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
