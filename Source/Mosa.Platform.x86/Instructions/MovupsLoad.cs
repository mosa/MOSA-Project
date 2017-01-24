// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 MovupsLoad instruction.
	/// </summary>
	public sealed class MovupsLoad : X86Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MovupsLoad"/>.
		/// </summary>
		public MovupsLoad() :
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
			MovupsMemoryToReg(node, emitter);
		}

		private static void MovupsMemoryToReg(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			int patchOffset;

			// mem to xmmreg1 0000 1111:0001 0000: mod xmmreg r/m
			var opcode = new OpcodeEncoder()
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendNibble(Bits.b1111)                                       // 4:opcode
				.AppendNibble(Bits.b0001)                                       // 4:opcode
				.AppendNibble(Bits.b0000)                                       // 4:opcode
				.AppendMod(true, node.Operand2)                                 // 2:mod
				.AppendRegister(node.Result.Register)                           // 3:register (destination)
				.AppendRM(node.Operand1)                                        // 3:r/m (source)
				.AppendConditionalDisplacement(!node.Operand2.IsConstantZero, node.Operand2)    // 8/32:displacement value
				.AppendConditionalPatchPlaceholder(node.Operand1.IsLinkerResolved, out patchOffset); // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, patchOffset);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
