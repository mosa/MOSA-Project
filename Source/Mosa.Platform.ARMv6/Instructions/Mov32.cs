// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Mov instruction: Copies operand to destination
	/// "Has only one operand. Constant support is limited to loading an 8-bit immediate value in ARMv6-M. If the operand is a shifted register, the instruction is an LSL, LSR, ASR, or ROR instruction instead."
	/// </summary>
	public class Mov32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mov32"/>.
		/// </summary>
		public Mov32() :
			base(1, 3)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(InstructionNode node, ARMv6CodeEmitter emitter)
		{
			EmitDataProcessingInstruction(node, emitter, Bits.b1101);
		}

		public override string __emitter { get { return "EmitDataProcessingInstruction"; } }
		public override string __bits { get { return "b1101"; } }
		public override string __description { get { return "Copies operand to destination"; } }
		public override string __description2 { get { return "Has only one operand. Constant support is limited to loading an 8-bit immediate value in ARMv6-M. If the operand is a shifted register, the instruction is an LSL, LSR, ASR, or ROR instruction instead."; } }

		#endregion Methods
	}
}
