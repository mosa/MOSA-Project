// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Swi instruction:
	/// Instruction is used to cause an SVCall (The Supervisor Call) exception to occur
	/// </summary>
	public class Swi32 : ARMv6Instruction
	{
		public override string __description { get { return "Supervisor Call"; } }
		public override string __description2 { get { return "Instruction is used to cause an SVCall (The Supervisor Call) exception to occur"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Swi32"/>.
		/// </summary>
		public Swi32() :
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
