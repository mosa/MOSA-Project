// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 compare-exchange instruction.
	/// </summary>
	public sealed class CmpXchgLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="CmpXchgLoad"/>.
		/// </summary>
		public CmpXchgLoad() :
			base(0, 3)
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
			CmpXchg(node, emitter);
		}

		private static void CmpXchg(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsRegister);
			Debug.Assert(node.Operand1.IsRegister);
			Debug.Assert(node.Operand2.IsRegister);
			Debug.Assert(node.GetOperand(3).IsRegister);
			Debug.Assert(node.Result.Register == GeneralPurposeRegister.EAX);
			Debug.Assert(node.Operand1.Register == GeneralPurposeRegister.EAX);
			Debug.Assert(node.ResultCount == 1);

			var linkreference = node.Operand2.IsLabel || node.Operand2.IsField || node.Operand2.IsSymbol;

			// Compare EAX with r/m32. If equal, ZF is set and r32 is loaded into r/m32.
			// Else, clear ZF and load r/m32 into EAX.

			// memory, register 0000 1111 : 1011 000w : mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(0x66, node.Size == InstructionSize.Size16)  // 8:prefix: 16bit
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b1011)                                       // 4:opcode
				.Append3Bits(Bits.b000)                                         // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)             // 1:width
				.ModRegRMSIBDisplacement(node.GetOperand(3), node.Operand2, node.Operand3) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(0, linkreference);               // 32:memory

			if (linkreference)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
