// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovssLoad instruction.
	/// </summary>
	public sealed class MovssLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovssLoad"/>.
		/// </summary>
		public MovssLoad() :
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
			MovssMemoryToReg(node, emitter);
		}

		private static void MovssMemoryToReg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// mem to xmmreg1 1111 0011:0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0011)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out int patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
