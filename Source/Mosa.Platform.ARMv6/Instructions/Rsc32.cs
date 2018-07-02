// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Rsc instruction: Reverse Subtract Carry
	/// Subtracts second operand from first operand plus carry minus 1. ARMv6-M only supports an immediate value of 0.
	/// </summary>
	public class Rsc32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rsb32"/>.
		/// </summary>
		public Rsc32() :
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
			EmitDataProcessingInstruction(node, emitter, Bits.b0111);
		}

		public override string __emitter { get { return "EmitDataProcessingInstruction"; } }
		public override string __bits { get { return "b0111"; } }
		public override string __description { get { return "Reverse Subtract Carry"; } }
		public override string __description2 { get { return "Subtracts second operand from first operand plus carry minus 1. ARMv6-M only supports an immediate value of 0."; } }

		#endregion Methods
	}
}
