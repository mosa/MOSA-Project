// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 Pextrd instruction.
	/// </summary>
	public sealed class Pextrd : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Pextrd"/>.
		/// </summary>
		public Pextrd() :
			base(1, 2)
		{
		}

		#endregion Construction

		#region Properties

		public override bool ThreeTwoAddressConversion { get { return false; } }

		#endregion Properties

		#region Methods

		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);
			Debug.Assert(node.Operand1.IsRegister);
			Debug.Assert(node.Operand2.IsConstant);

			// reg from xmmreg, imm8
			// 0110 0110:0000 1111:0011 1010: 0001 0110:11 xmmreg reg: imm8
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0110)                                       // 4:opcode
				.AppendNibble(Bits.b0110)                                       // 4:opcode

				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode

				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b1010)                                       // 4:opcode

				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0110)                                       // 4:opcode

				.Append2Bits(Bits.b11)                                          // 2:opcode
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendRegister(node.Result.Register)                           // 3:register (destination)

				.AppendByteValue((byte)node.Operand2.ConstantUnsignedInteger);  // 8:memory

			emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
