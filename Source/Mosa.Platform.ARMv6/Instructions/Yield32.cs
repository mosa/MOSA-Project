// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Yield instruction: Yield
	/// </summary>
	public class Yield32 : ARMv6Instruction
	{
		public override string __description { get { return "Yield"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Yield32"/>.
		/// </summary>
		public Yield32() :
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
