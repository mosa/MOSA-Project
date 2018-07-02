// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Adr instruction: Form PC-relative Address
	/// First operand is the PC. Second operand is an immediate constant.
	/// </summary>
	public class Adr32 : ARMv6Instruction
	{
		public override string __description { get { return "Form PC-relative Address"; } }
		public override string __description2 { get { return "First operand is the PC. Second operand is an immediate constant."; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Adr32"/>.
		/// </summary>
		public Adr32() :
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
