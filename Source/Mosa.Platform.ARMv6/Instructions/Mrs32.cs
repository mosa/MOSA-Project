// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Mrs instruction: "move the contents of the Application Program Status Register, APSR, to or from a general-purpose register."
	/// Status register access instructions
	/// </summary>
	public class Mrs32 : ARMv6Instruction
	{
		public override string __description { get { return "Status register access"; } }
		public override string __description2 { get { return "Move the contents of the Application Program Status Register, APSR, to or from a general-purpose register."; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mrs32"/>.
		/// </summary>
		public Mrs32() :
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
			// TODO
		}

		#endregion Methods
	}
}
