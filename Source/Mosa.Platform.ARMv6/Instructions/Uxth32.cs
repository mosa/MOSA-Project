// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Uxth instruction: Unsigned Extend Halfword
	/// Extend 16 bits to 32
	/// </summary>
	public class Uxth32 : ARMv6Instruction
	{
		public override string __description { get { return "Unsigned Extend Halfword"; } }
		public override string __description2 { get { return "Extend 16 bits to 32"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Uxth32"/>.
		/// </summary>
		public Uxth32() :
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
