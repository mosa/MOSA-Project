// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovupsStore instruction.
	/// </summary>
	public sealed class MovupsStore : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovupsStore"/>.
		/// </summary>
		public MovupsStore() :
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
			MovupsRegToMemory(node, emitter);
		}

		private static void MovupsRegToMemory(InstructionNode node, MachineCodeEmitter emitter)
		{
			Debug.Assert(node.Operand3.IsCPURegister);
			Debug.Assert(node.ResultCount == 0);
			Debug.Assert(!node.Operand3.IsConstant);

			int patchOffset;

			// xmmreg1 to mem 0000 1111:0001 0001: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode

				// This opcode has a directionality bit, and it is set to 0
				// This means we must swap around operand1 and operand3, and set offsetDestination to false
				.ModRegRMSIBDisplacement(false, node.Operand3, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement

				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset);  // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
