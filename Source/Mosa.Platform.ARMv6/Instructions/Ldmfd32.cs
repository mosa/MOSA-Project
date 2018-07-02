// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv6.Instructions
{
	/// <summary>
	/// Ldmfd instruction: Load Multiple Full Descending
	/// </summary>
	public class Ldmfd32 : ARMv6Instruction
	{
		public override string __description { get { return "Load Multiple Full Descending"; } }

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Ldmfd32"/>.
		/// </summary>
		public Ldmfd32() :
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
