// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	///
	/// </summary>
	public sealed class Lea : TwoOperandInstruction
	{
		#region Properties

		/// <summary>
		/// Gets a value indicating whether [three two address conversion].
		/// </summary>
		/// <value>
		/// <c>true</c> if [three two address conversion]; otherwise, <c>false</c>.
		/// </value>
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
			LeaAddress(node, emitter);
		}

		private static void LeaAddress(InstructionNode node, BaseCodeEmitter emitter)
		{
			Debug.Assert(node.Result.IsCPURegister);

			// LEA – Load Effective Address 1000 1101 : modA reg r/m
			var opcode = new OpcodeEncoder()
				.AppendConditionalPrefix(node.Size == InstructionSize.Size16, 0x66)  // 8:prefix: 16bit
				.AppendNibble(Bits.b1000)                                       // 4:opcode
				.AppendNibble(Bits.b1101)                                       // 3:opcode
				.ModRegRMSIBDisplacement(false, node.Result, node.Operand1, node.Operand2) // Mod-Reg-RM-?SIB-?Displacement
				.AppendConditionalIntegerValue(node.Operand1.IsLinkerResolved, 0);               // 32:memory

			if (node.Operand1.IsLinkerResolved)
				emitter.Emit(opcode, node.Operand1, (opcode.Size - 32) / 8);
			else
				emitter.Emit(opcode);
		}

		#endregion Methods
	}
}
