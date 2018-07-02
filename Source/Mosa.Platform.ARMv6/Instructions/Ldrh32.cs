// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldrh instruction: Load 16-bit unsigned halfword
	/// Load and store instructions
	/// </summary>
	public class Ldrh32 : ARMv6Instruction
	{
		public override string __description { get { return "Load 16-bit unsigned halfword"; } }
		public override string __description2 { get { return "Load and store instructions"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Ldrh32"/>.
		/// </summary>
		public Ldrh32() :
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
