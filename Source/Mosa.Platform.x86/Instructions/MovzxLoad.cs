// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovzxLoad instruction.
	/// </summary>
	public sealed class MovzxLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovzxLoad"/>.
		/// </summary>
		public MovzxLoad() :
			base(1, 2)
		{
		}

		#endregion Construction

		#region Properties

		public override bool ThreeTwoAddressConversion { get { return false; } }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		public override void Emit(InstructionNode node, BaseCodeEmitter emitter)
		{
			MovzxMemoryToReg(node, emitter);
		}

		private static void MovzxMemoryToReg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// WARNING: DO NOT USE 0x66 PREFIX WITH THIS INSTRUCTION
			// We currently don't have the ability to load into 16bit registers

			// memory to register 0000 1111 : 1011 011w: mod reg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                           // 4:opcode
				.AppendNibble(Bits.b1111)                                           // 4:opcode
				.AppendNibble(Bits.b1011)                                           // 4:opcode
				.Append3Bits(Bits.b011)                                             // 3:opcode
				.AppendWidthBit(node.Size != InstructionSize.Size8)                 // 1:width
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset) // 32:memory
				.AppendConditionalIntegerValue(node.Operand1.IsConstant && !node.Operand1.IsLinkerResolved, node.Operand1.ConstantUnsignedInteger); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
