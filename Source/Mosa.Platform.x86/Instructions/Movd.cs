// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Movd instruction.
	/// </summary>
	public sealed class Movd : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Pextrd"/>.
		/// </summary>
		public Movd() :
			base(1, 1)
		{
		}

		#endregion Construction

		#region Methods

		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);
			Debug.Assert(node.Operand1.IsRegister);

			// reg from mmxreg
			// 0000 1111:0111 1110: 11 mmxreg reg
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode

				.Append3Bits(Bits.b011)                                         // 3:opcode
				.AppendBit(node.Result.Register.Width != 128)                   // 1:direction
				.AppendNibble(Bits.b1110)                                       // 4:opcode

				.Append2Bits(Bits.b11)                                          // 2:opcode
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendRegister(node.Result.Register);                          // 3:register (destination)

			emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
