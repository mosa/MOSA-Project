// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Bkpt instruction: Breakpoint instruction
	/// It can cause a running system to halt depending on the debug configuration.
	/// </summary>
	public class Bkpt32 : ARMv6Instruction
	{
		public override string __description { get { return "Breakpoint instruction"; } }
		public override string __description2 { get { return "It can cause a running system to halt depending on the debug configuration."; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Bkpt32"/>.
		/// </summary>
		public Bkpt32() :
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
