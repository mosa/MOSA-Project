// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Rsb instruction: Reverse Subtract
	/// Subtracts first operand from second operand. ARMv6-M only supports an immediate value of 0.
	/// </summary>
	public class Rsb32 : ARMv6Instruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Rsb32"/>.
		/// </summary>
		public Rsb32() :
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
			EmitDataProcessingInstruction(node, emitter, Bits.b0011);
		}

		public override string __emitter { get { return "EmitDataProcessingInstruction"; } }
		public override string __bits { get { return "b0011"; } }
		public override string __description { get { return "Reverse Subtract"; } }
		public override string __description2 { get { return "Subtracts first operand from second operand. ARMv6-M only supports an immediate value of 0."; } }

		#endregion Methods
	}
}
