// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Lsr instruction: Logical Shift Right
	/// </summary>
	public class Lsr32 : ARMv6Instruction
	{
		public override string __description { get { return "Logical Shift Right"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Lsr32"/>.
		/// </summary>
		public Lsr32() :
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
