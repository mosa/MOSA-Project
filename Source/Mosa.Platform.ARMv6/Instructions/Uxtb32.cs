// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Uxtb instruction: Unsigned Extend Byte
	/// Extend 8 bits to 32
	/// </summary>
	public class Uxtb32 : ARMv6Instruction
	{
		public override string __description { get { return "Unsigned Extend Byte"; } }
		public override string __description2 { get { return "Extend 8 bits to 32"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Uxtb32"/>.
		/// </summary>
		public Uxtb32() :
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
