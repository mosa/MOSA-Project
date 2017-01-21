// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovsdLoad instruction.
	/// </summary>
	public sealed class MovsdLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovsdLoad"/>.
		/// </summary>
		public MovsdLoad() :
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
		protected override void Emit(InstructionNode node, MachineCodeEmitter emitter)
		{
			MovsdMemoryToReg(node, emitter);
		}

		private static void MovsdMemoryToReg(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			int patchOffset;

			// mem to xmmreg1 1111 0010:0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0010)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
